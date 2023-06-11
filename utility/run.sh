#!/bin/bash

# set -x

export GitLab__BuildPack__ExecutorBranch="$commit_branch"

# Start the first process
dotnet Seacraft.Web.dll &
status=$?
if [ $status -ne 0 ]; then
  echo "Failed to start dotnet process: $status"
  exit $status
fi

# Naive check runs checks once a minute to see if either of the processes exited.
# This illustrates part of the heavy lifting you need to do if you want to run
# more than one service in a container. The container exits with an error
# if it detects that either of the processes has exited.
# Otherwise it loops forever, waking up every 60 seconds

while sleep 60; do
  ps aux |grep dotnet |grep -q -v grep
  PROCESS_STATUS=$?
  # If the greps above find anything, they exit with 0 status
  # If they are not both 0, then something is wrong
  if [ $PROCESS_STATUS -ne 0 ]; then
    echo "The processes has already exited."
    exit 1
  fi
done

trap : TERM INT; sleep infinity & wait
