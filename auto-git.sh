#!/bin/sh

if [ "$1" != "" ] && [ "$2" != "" ]
then
	echo "****************************************************"
	echo "                       STATUS"
	echo "****************************************************"
	echo
	git status
	echo
	echo "****************************************************"
	echo "                  ADD MODIFICATION"
	echo "****************************************************"
	echo
	git add .
	echo
	echo "****************************************************"
	echo "                     NEW STATUS"
	echo "****************************************************"
	echo
	git status
	echo
	echo "****************************************************"
	echo "                 COMMIT MODIFICATION"
	echo "****************************************************"
	echo
	git commit -m "$1"
	echo
	echo "****************************************************"
	echo "                   PUSH MODIFICATION"
	echo "****************************************************"
	echo
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
