import sys
import os
import argparse
import shutil


def do_zip(args):
  print(os.getcwd())
  directory_to_zip = args.directory
  output_file = args.outfile
  name_and_ext = os.path.splitext(output_file)
  name = name_and_ext[0]

  shutil.make_archive(name, 'zip', directory_to_zip)


if __name__ == "__main__":
  parser = argparse.ArgumentParser("zip_lambda")
  parser.add_argument("directory", help="Directory to ZIP")
  parser.add_argument("outfile", help="Output file name")
  do_zip(parser.parse_args())
