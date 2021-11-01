#!/bin/sh

if [ "$1" != "" ] && [ "$2" != "" ]
then
	git status
	git add .
	git status
	git commit -m "$1"
	git push -u origin "$2"
else
	if [ "$1" == "" ]
	then
		echo "Commit message is missing."

	elif [ "$2" == "" ]
	then
		echo "Git branch to push modification in is missing."
	fi
fi
