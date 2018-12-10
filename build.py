import re
import os
import sys
import argparse
import subprocess

parser = argparse.ArgumentParser(add_help=True)
parser.add_argument('--git',          action='store_true', dest='git',          help='Use git to release builded project')
parser.add_argument('--build',          action='store_true', dest='build',          help='Build project')
parser.add_argument('--new',          action='store_true', dest='new',          help='Update version number')

args = parser.parse_args()

version =''

if args.git:
    tag_cmd = ["git", "tag"]
    tag_proc = subprocess.Popen(tag_cmd, stdout=subprocess.PIPE)
    # tag_proc = subprocess.Popen(tag_cmd)
    output, _ = tag_proc.communicate()

    output = output.decode('utf-8')
    last_version = re.findall(r'v\d.\d', output)[-1]
    if args.new:
        version = 'v{:1.1f}'.format(float(last_version[1:]) + 0.1)
    else:
        version = last_version
    with open('.\GUI\WindowsFormsApp1\Properties\AssemblyInfo.cs', 'rb') as file:
        data = file.readlines()
    data[-1] = b'[assembly: AssemblyFileVersion("' + version[1:].encode()+ b'")]'
    with open('.\GUI\WindowsFormsApp1\Properties\AssemblyInfo.cs', 'wb') as file:
        file.writelines(data)
    with open('config.cfg', 'r') as file:
        data = file.readlines()
    data[-1] = 'VERSION={}'.format(version[1:])
    with open('config.cfg', 'w') as file:
        file.writelines(data)

if args.build:
    command = ['build.bat']
    p = subprocess.Popen(command, stdout=subprocess.PIPE)
    p.communicate()
    print('終わりました')

if args.git:
    add_cmd = ["git", "add", "."]
    subprocess.Popen(add_cmd).communicate()
    print("GIT: Files added!")

    commit_cmd = ["git", "commit", "-m", "version:{}".format(version)]
    subprocess.Popen(commit_cmd).communicate()
    print("GIT: Files commited!")

    new_tag_cmd = ["git", "tag", version]
    subprocess.Popen(new_tag_cmd).communicate()
    print("GIT: Tag created!")

    push_cmd = ["git", "push"]
    subprocess.Popen(push_cmd).communicate()
    print("GIT: Commit pushed!")

    push_tag_cmd = ["git", "push", "origin", version]
    subprocess.Popen(push_tag_cmd).communicate()
    print("GIT: Tag pushed!")






