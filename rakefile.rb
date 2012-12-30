COPYRIGHT = "Copyright 2012 Chris Patterson, All rights reserved."

internal_files = Dir[File.join(File.expand_path("src"), 'FeatherVane/Internals/**/*.cs')]
if(!internal_files.any?)
  #you didn't git submodule update --init - here let me help you
  sh 'git submodule update --init' unless internal_files.any?
end

require File.dirname(__FILE__) + "/build_support/BuildUtils.rb"
require File.dirname(__FILE__) + "/build_support/util.rb"
include FileTest
require 'albacore'
require File.dirname(__FILE__) + "/build_support/versioning.rb"

PRODUCT = 'FeatherVane'
CLR_TOOLS_VERSION = 'v4.0.30319'
OUTPUT_PATH = 'bin/Release'

props = {
  :src => File.expand_path("src"),
  :output => File.expand_path("build_output"),
  :artifacts => File.expand_path("build_artifacts"),
  :projects => ["FeatherVane"],
  :lib => File.expand_path("lib")
}

desc "Cleans, compiles, il-merges, unit tests, prepares examples, packages zip"
task :all => [:default, :package]

desc "**Default**, compiles and runs tests"
task :default => [:clean, :nuget_restore, :compile, :tests, :package]

desc "Update the common version information for the build. You can call this task without building."
assemblyinfo :global_version do |asm|
  # Assembly file config
  asm.product_name = PRODUCT
  asm.description = "FeatherVane, Lightweight Middleware for .NET Applications"
  asm.version = FORMAL_VERSION
  asm.file_version = FORMAL_VERSION
  asm.custom_attributes :AssemblyInformationalVersion => "#{BUILD_VERSION}",
	:ComVisibleAttribute => false,
	:CLSCompliantAttribute => true
  asm.copyright = COPYRIGHT
  asm.output_file = 'src/SolutionVersion.cs'
  asm.namespaces "System", "System.Reflection", "System.Runtime.InteropServices", "System.Security"
end

desc "Prepares the working directory for a new build"
task :clean do
	FileUtils.rm_rf props[:output]
	waitfor { !exists?(props[:output]) }

	FileUtils.rm_rf props[:artifacts]
	waitfor { !exists?(props[:artifacts]) }

	Dir.mkdir props[:output]
	Dir.mkdir props[:artifacts]
end

desc "Cleans, versions, compiles the application and generates build_output/."
task :compile => [:versioning, :global_version, :build] do
	copyOutputFiles File.join(props[:src], "FeatherVane/bin/Release"), "FeatherVane.{dll,pdb,xml}", File.join(props[:output], 'net-4.0')
	copyOutputFiles File.join(props[:src], "FeatherVane.Web/bin/Release"), "FeatherVane.Web.{dll,pdb,xml}", File.join(props[:output], 'net-4.0')
end

desc "Cleans, versions, compiles the application and generates build_output/."
task :compile_net45fx => [:global_version, :build_net45fx] do
  copyOutputFiles File.join(props[:src], "FeatherVane/bin/Release"), "FeatherVane.{dll,pdb,xml}", File.join(props[:output], 'win8')
end

desc "Only compiles the application."
msbuild :build do |msb|
	msb.properties :Configuration => "Release",
		:Platform => 'Any CPU'
	msb.use :net4
	msb.targets :Clean, :Build
  msb.properties[:SignAssembly] = 'true'
  msb.properties[:AssemblyOriginatorKeyFile] = props[:keyfile]
	msb.solution = 'src/FeatherVane.sln'
end

desc "Only compiles the application for .NET 4.5 FX CORE."
msbuild :build_net45fx do |msb|
  msb.properties :Configuration => "Release",
    :Platform => 'Any CPU'
  msb.targets :Clean, :Build
  msb.properties[:SignAssembly] = 'true'
  msb.properties[:AssemblyOriginatorKeyFile] = props[:keyfile]
  msb.solution = 'src/FeatherVane-NetCore45.sln'
end

def copyOutputFiles(fromDir, filePattern, outDir)
	FileUtils.mkdir_p outDir unless exists?(outDir)
	Dir.glob(File.join(fromDir, filePattern)){|file|
		copy(file, outDir) if File.file?(file)
	}
end

desc "Runs unit tests"
nunit :tests => [:compile] do |nunit|

          nunit.command = File.join('src', 'packages','NUnit.Runners.2.6.1', 'tools', 'nunit-console.exe')
          nunit.options = "/framework=#{CLR_TOOLS_VERSION}", '/nothread', '/nologo', '/labels', "\"/xml=#{File.join(props[:artifacts], 'nunit-test-results.xml')}\""
          nunit.assemblies = FileList[File.join(props[:src], "FeatherVane.Tests/bin/Release", "FeatherVane.Tests.dll")]
end

task :package => [:nuget, :zip_output]

desc "ZIPs up the build results."
zip :zip_output => [:versioning] do |zip|
	zip.directories_to_zip = [props[:output]]
	zip.output_file = "FeatherVane-#{NUGET_VERSION}.zip"
	zip.output_path = props[:artifacts]
end

desc "Restore NuGet Packages"
task :nuget_restore do
  sh "lib/nuget install #{File.join(props[:src],"FeatherVane.Tests","packages.config")} -o #{File.join(props[:src],"packages")}"
end

desc "Builds the nuget package"
task :nuget => [:versioning, :create_nuspec] do
	sh "lib/nuget pack #{props[:artifacts]}/FeatherVane.nuspec /Symbols /OutputDirectory #{props[:artifacts]}"
	sh "lib/nuget pack #{props[:artifacts]}/FeatherVane.Web.nuspec /Symbols /OutputDirectory #{props[:artifacts]}"
end

task :create_nuspec => [:_nuspec]

nuspec :_nuspec do |nuspec|
  nuspec.id = 'FeatherVane'
  nuspec.version = NUGET_VERSION
  nuspec.authors = 'Chris Patterson'
  nuspec.description = 'FeatherVane, Lightweight Middleware for .NET Applications'
  nuspec.title = 'FeatherVane'
  nuspec.projectUrl = 'http://github.com/TheOrangeBook/FeatherVane'
  nuspec.language = "en-US"
  nuspec.licenseUrl = "http://www.apache.org/licenses/LICENSE-2.0"
  nuspec.requireLicenseAcceptance = "false"
  nuspec.output_file = File.join(props[:artifacts], 'FeatherVane.nuspec')
  add_files props[:output], 'FeatherVane.{dll,pdb,xml}', nuspec
  nuspec.file(File.join(props[:src], "FeatherVane\\**\\*.cs").gsub("/","\\"), "src")
end

nuspec :_nuspec do |nuspec|
  nuspec.id = 'FeatherVane.Web'
  nuspec.version = NUGET_VERSION
  nuspec.authors = 'Chris Patterson'
  nuspec.description = 'FeatherVane, Lightweight Middleware for .NET Applications. Support for System.Web services, including IIS and HttpListener'
  nuspec.title = 'FeatherVane.Web'
  nuspec.projectUrl = 'http://github.com/TheOrangeBook/FeatherVane'
  nuspec.language = "en-US"
  nuspec.licenseUrl = "http://www.apache.org/licenses/LICENSE-2.0"
  nuspec.requireLicenseAcceptance = "false"
  nuspec.dependency "FeatherVane", NUGET_VERSION
  nuspec.output_file = File.join(props[:artifacts], 'FeatherVane.Web.nuspec')
  add_files props[:output], 'FeatherVane.Web.{dll,pdb,xml}', nuspec
  nuspec.file(File.join(props[:src], "FeatherVane.Web\\**\\*.cs").gsub("/","\\"), "src")
end

def project_outputs(props)
	props[:projects].map{ |p| "src/#{p}/bin/#{BUILD_CONFIG}/#{p}.dll" }.
		concat( props[:projects].map{ |p| "src/#{p}/bin/#{BUILD_CONFIG}/#{p}.exe" } ).
		find_all{ |path| exists?(path) }
end

def get_commit_hash_and_date
	begin
		commit = `git log -1 --pretty=format:%H`
		git_date = `git log -1 --date=iso --pretty=format:%ad`
		commit_date = DateTime.parse( git_date ).strftime("%Y-%m-%d %H%M%S")
	rescue
		commit = "git unavailable"
	end

	[commit, commit_date]
end

def add_files stage, what_dlls, nuspec
  [['net40', 'net-4.0'], ['.NETCore45', 'win8']].each{|fw|
    takeFrom = File.join(stage, fw[1], what_dlls)
    Dir.glob(takeFrom).each do |f|
      nuspec.file(f.gsub("/", "\\"), "lib\\#{fw[0]}")
    end
  }
end

def waitfor(&block)
	checks = 0

	until block.call || checks >10
		sleep 0.5
		checks += 1
	end

	raise 'Waitfor timeout expired. Make sure that you aren\'t running something from the build output folders, or that you have browsed to it through Explorer.' if checks > 10
end
