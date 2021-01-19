import os
import argparse
import subprocess


def section(name):
    print()
    print('-'*72)
    print("{:^72s}".format(name))
    print('-'*72)


class DeployNonServiceResource:
    def __init__(self, args):
        self.resource = args.resource
        self.tfvar_defaults_file_path = os.path.join(
            "tfvars", "defaults.tfvars.json")
        self.tfvar_file_path = os.path.join(
            "tfvars", f"{self.resource}.tfvars.json")

    def run(self):
        self.__stage_apply_terraform()

    def __stage_apply_terraform(self):
        section(f'Running Terraform')
        tf_cmd = [
            "terraform",
            "apply",
            "-auto-approve",
            f"-var-file={self.tfvar_defaults_file_path}",
            f"-var-file={self.tfvar_file_path}",
            f"-target=module.{self.resource}"
        ]
        tf_cmd_result = subprocess.run(tf_cmd, cwd="tf")
        if tf_cmd_result.returncode != 0:
            return False
        print('Done.')
        return True


def run_script(args):
    deploy = DeployNonServiceResource(args)
    deploy.run()


if __name__ == "__main__":
    parser = argparse.ArgumentParser("deploy_non_service_resource")
    parser.add_argument("--resource", help="Resource to deploy",
                        choices=["employee_document_db", "employee_merge_save_data_bus"])
    run_script(parser.parse_args())
