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

all_services = list(service_to_proj.keys())


def section(name):
    print()
    print('-'*72)
    print("{:^72s}".format(name))
    print('-'*72)


class DeployService:
    def __get_value_or_override(self, attribute, args, overrides):
        if attribute in overrides:
            return overrides[attribute]
        return args.__dict__[attribute]

    def __init__(self, args, overrides):
        self.cleanup = True
        if self.__get_value_or_override('cleanup', args, overrides) != "true":
            self.cleanup = False

        self.service = self.__get_value_or_override('service', args, overrides)
        self.project_location = service_to_proj[self.service]
        self.zip_pkg_name = f"{self.service}_pkg.zip"
        self.zip_pkg_path = os.path.join(
            self.project_location, self.zip_pkg_name)
        self.zip_pkg_target_location = os.path.join("tf", self.zip_pkg_name)
        self.tfvar_defaults_file_path = os.path.join(
            "tfvars", "defaults.tfvars.json")
        self.tfvar_file_path = os.path.join(
            "tfvars", f"{self.service}.tfvars.json")
        pass

    def run(self):
        stages = [self.__stage_pre_clean, self.__stage_packaging,
                  self.__stage_copy_zip, self.__stage_apply_terraform, self.__stage_post_clean]
        for fn in stages:
            passed = fn()
            if not passed:
                break

    def __stage_pre_clean(self):
        section(f'Cleaning {self.project_location}')
        clean_cmd = ["dotnet", "clean"]
        clean_cmd_result = subprocess.run(clean_cmd, cwd="Services")
        if clean_cmd_result.returncode != 0:
            return False
        print('Done.')
        return True

    def __stage_packaging(self):
        section(f'Packaging {self.project_location}')
        package_cmd = ["dotnet", "lambda", "package",
                       self.zip_pkg_name, "-pt zip", "-c Release"]
        pkg_cmd_result = subprocess.run(package_cmd, cwd=self.project_location)
        if pkg_cmd_result.returncode != 0:
            return False
        print('Done.')
        return True

    def __stage_copy_zip(self):
        section(f'Copying ZIP package closer to the Terraform')
        shutil.move(self.zip_pkg_path, self.zip_pkg_target_location)
        print('Done.')
        return True

    def __stage_apply_terraform(self):
        section(f'Running Terraform')
        tf_cmd = [
            "terraform",
            "apply",
            "-auto-approve",
            f"-var-file={self.tfvar_defaults_file_path}",
            f"-var-file={self.tfvar_file_path}",
            f"-target=module.{self.service}"
        ]
        tf_cmd_result = subprocess.run(tf_cmd, cwd="tf")
        if tf_cmd_result.returncode != 0:
            return False
        print('Done.')
        return True

    def __stage_post_clean(self):
        if self.cleanup:
            section("Cleaning up")
            os.remove(self.zip_pkg_target_location)
            print('Done.')
        return True


def run_script(args):
    services_to_deploy = []
    if args.service == "all":
        services_to_deploy = [] + all_services
    else:
        services_to_deploy = [args.service]

    for svc in services_to_deploy:
        deploy = DeployService(args, {'service': svc})
        deploy.run()


if __name__ == "__main__":
    parser = argparse.ArgumentParser("deploy_service")
    parser.add_argument("--service", help="Service to deploy", default="all",
                        choices=["update_employee", "merge_employee", "fetch_employee", "all"])
    parser.add_argument(
        "--cleanup", help="Whether to remove the ZIP pkg after deploy.", default="true")
    run_script(parser.parse_args())
