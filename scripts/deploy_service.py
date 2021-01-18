import os
import argparse
import subprocess
import shutil
import time

DOTNET_TARGET = "netcoreapp3.1"

service_to_proj = {
  "update_employee": os.path.join("Services", "UpdateEmployee"),
  "fetch_employee": os.path.join("Services", "FetchEmployee"),
  "merge_employee": os.path.join("Services", "MergeEmployee")
}

def section(name):
  print()
  print('-'*72)
  print("{:^72s}".format(name))
  print('-'*72)


def create_package(args):
  cleanup = True if args.cleanup == "true" else False
  cwd = os.getcwd()
  service = args.service
  project_location = service_to_proj[service]
  
  section(f'Cleaning {project_location}')
  clean_cmd = ["dotnet", "clean"]
  clean_cmd_result = subprocess.run(clean_cmd, cwd="Services")
  print('Done.')

  section(f'Packaging {project_location}')
  zip_pkg_name = f"{service}_pkg.zip"
  package_cmd = ["dotnet", "lambda", "package", zip_pkg_name, "-pt zip", "-c Release"]
  pkg_cmd_result = subprocess.run(package_cmd, cwd=project_location)
  print('Done.')

  section(f'Copying ZIP package closer to the Terraform')
  zip_pkg_path = os.path.join(project_location, zip_pkg_name)
  zip_pkg_target_location = os.path.join("tf", zip_pkg_name)

  shutil.move(zip_pkg_path, zip_pkg_target_location)
  print('Done.')

  section(f'Running Terraform')
  tfvar_file = os.path.join("tfvars", f"{service}.tfvars.json")
  tf_cmd = [
    "terraform", 
    "apply", 
    "-auto-approve",
    f"-var-file={tfvar_file}",
    f"-target=module.{service}"
    ]
  tf_cmd_result = subprocess.run(tf_cmd, cwd="tf")
  print('Done.')

  if cleanup:
    section("Cleaning up")
    os.remove(zip_pkg_target_location)
    print('Done.')


if __name__ == "__main__":
  parser = argparse.ArgumentParser("deploy_service")
  parser.add_argument("--service", help="Service to deploy", choices=["update_employee", "merge_employee", "fetch_employee"])
  parser.add_argument("--cleanup", help="Whether to remove the ZIP pkg after deploy.", default="true")
  create_package(parser.parse_args())
