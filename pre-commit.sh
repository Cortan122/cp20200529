#!/bin/sh

FILES="$(git diff --cached --name-only --diff-filter=ACM "*.cs")"
[ -z "$FILES" ] && exit 0

dotnet.exe format --files "$(echo "$FILES" | tr '\n' ',')"

echo "$FILES" | tr '\n' '\0' | xargs -0 git add

exit 0
