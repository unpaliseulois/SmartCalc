#!/bin/sh

# Reset
ResetColor='\033[0m'       # Text Reset

# Regular Colors
Black='\033[0;30m'        # Black
Red='\033[0;31m'          # Red
Green='\033[0;32m'        # Green
Yellow='\033[0;33m'       # Yellow
Blue='\033[0;34m'         # Blue
Purple='\033[0;35m'       # Purple
Cyan='\033[0;36m'         # Cyan
White='\033[0;37m'        # White

# Bold
BBlack='\033[1;30m'       # Black
BRed='\033[1;31m'         # Red
BGreen='\033[1;32m'       # Green
BYellow='\033[1;33m'      # Yellow
BBlue='\033[1;34m'        # Blue
BPurple='\033[1;35m'      # Purple
BCyan='\033[1;36m'        # Cyan
BWhite='\033[1;37m'       # White

# Underline
UBlack='\033[4;30m'       # Black
URed='\033[4;31m'         # Red
UGreen='\033[4;32m'       # Green
UYellow='\033[4;33m'      # Yellow
UBlue='\033[4;34m'        # Blue
UPurple='\033[4;35m'      # Purple
UCyan='\033[4;36m'        # Cyan
UWhite='\033[4;37m'       # White

# Background
On_Black='\033[40m'       # Black
On_Red='\033[41m'         # Red
On_Green='\033[42m'       # Green
On_Yellow='\033[43m'      # Yellow
On_Blue='\033[44m'        # Blue
On_Purple='\033[45m'      # Purple
On_Cyan='\033[46m'        # Cyan
On_White='\033[47m'       # White

# High Intensity
IBlack='\033[0;90m'       # Black
IRed='\033[0;91m'         # Red
IGreen='\033[0;92m'       # Green
IYellow='\033[0;93m'      # Yellow
IBlue='\033[0;94m'        # Blue
IPurple='\033[0;95m'      # Purple
ICyan='\033[0;96m'        # Cyan
IWhite='\033[0;97m'       # White

# Bold High Intensity
BIBlack='\033[1;90m'      # Black
BIRed='\033[1;91m'        # Red
BIGreen='\033[1;92m'      # Green
BIYellow='\033[1;93m'     # Yellow
BIBlue='\033[1;94m'       # Blue
BIPurple='\033[1;95m'     # Purple
BICyan='\033[1;96m'       # Cyan
BIWhite='\033[1;97m'      # White

# High Intensity backgrounds
On_IBlack='\033[0;100m'   # Black
On_IRed='\033[0;101m'     # Red
On_IGreen='\033[0;102m'   # Green
On_IYellow='\033[0;103m'  # Yellow
On_IBlue='\033[0;104m'    # Blue
On_IPurple='\033[0;105m'  # Purple
On_ICyan='\033[0;106m'    # Cyan
On_IWhite='\033[0;107m'   # White

function PrintCenter(){
	if [ "$1" != "" ]
	then
		echo "$1" | sed  -e :a -e "s/^.\{1,$(tput cols)\}$/ & /;ta" | tr -d '\n' | head -c $(tput cols)
		return 0
	else
		return 1
	fi
}

display_file_center(){
    columns="$(tput cols)"
    while IFS= read -r line; do
        printf "%*s\n" $(( (${#line} + columns) / 2)) "$line"
    done < "$1"
}

display_file_right(){
    columns="$(tput cols)"
    while IFS= read -r line; do
        printf "%*s\n" $columns "$line"
    done < "$1"
}

Right(){
    columns="$(tput cols)"    
    printf "%*s\n" $columns "$1"
    
}

Center(){
	if [ "$1" != "" ]
	then
    	columns="$(tput cols)"    
    	printf "%*s\n" $(( (${#1} + columns) / 2)) "${1}"
    	return 0
	else
		return 1
	fi
}

function Line()
{ 
	local l=
 	builtin printf -vl "%${2:-${COLUMNS:-`tput cols 2>&-||echo 80`}}s\n" \
		&& echo -e "${l// /${1:-=}}"
}

function Banner()
{
	if [ "$#" -eq 2 ]
	then
		Line "${1}"
		Center "${2}"
		echo
		Line "${1}"
		return 0
	else
		echo -e "${BRed}"
		echo "Illegal number of parameters."
		echo " Usage: Banner <Border Char> <Banner Text>."
		echo -e "${ResetColor}"
		return 1
	fi
}

if [ "$1" != "" ] && [ "$2" != "" ]
then
	echo -e "${BBlue}"	
	Banner '-' 'STATUS'
	echo -e "${ResetColor}"

	git status

	echo -e "${BBlue}"
	Banner '-' 'ADD MODIFICATION'
	echo -e "${ResetColor}"

	git add .

	echo -e "${BBlue}"
	Banner '-' 'NEW STATUS'
	echo -e "${ResetColor}"

	git status

	echo -e "${BBlue}"
	Banner '-' 'COMMIT MODIFICATION'
	echo -e "${ResetColor}"

	git commit -m "$1"

	echo -e "${BBlue}"
	Banner '-' 'PUSH MODIFICATION'
	echo -e "${ResetColor}"

	git push -u origin "$2"

	echo
	echo "${BGreen}The git process has completed successfully."
	echo -e "${ResetColor}"

	return 0
else
	echo -e "${BRed}"
	if [ "$1" == "" ]
	then		
		echo "Commit message is missing."

	elif [ "$2" == "" ]
	then
		echo "Git branch to push modification in is missing."
	fi
	echo -e "${ResetColor}"
	return 0
fi



