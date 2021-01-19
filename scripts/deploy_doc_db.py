import os
import argparse
import subprocess


def section(name):
    print()
    print('-'*72)
    print("{:^72s}".format(name))
    print('-'*72)


class DeployDocDb:
    def __init__(self, args):
        self.database = args.database
        self.tfvar_defaults_file_path = os.path.join(
            "tfvars", "defaults.tfvars.json")
        self.tfvar_file_path = os.path.join(
            "tfvars", f"{self.database}.tfvars.json")

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
            f"-target=module.{self.database}"
        ]
        tf_cmd_result = subprocess.run(tf_cmd, cwd="tf")
        if tf_cmd_result.returncode != 0:
            return False
        print('Done.')
        return True


def run_script(args):
    deploy = DeployDocDb(args)
    deploy.run()


if __name__ == "__main__":
    parser = argparse.ArgumentParser("deploy_doc_db")
    parser.add_argument("--database", help="Service to deploy",
                        default="employee_document_db", choices=["employee_document_db"])
    run_script(parser.parse_args())
