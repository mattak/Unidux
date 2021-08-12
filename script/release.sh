#!/bin/sh

set -ue

if [ $# -lt 1 ]; then
  cat << __USAGE__
Usage: $0 <version>
Example:
  $0 0.4.2
__USAGE__

  exit 1
fi

VERSION=$1
TARGET_PACKAGE_JSON=Assets/Plugins/Unidux/Scripts/package.json

if [ "$(git status --short | wc -l | awk '{print $1}')" != "0" ]; then
  echo "ERROR: git status is not clean. Please commit or clean files"
  exit 1
fi

exit 0

cat "$TARGET_PACKAGE_JSON" | jq --arg version "$VERSION" '.version = $version' > /tmp/package.json
mv /tmp/package.json "$TARGET_PACKAGE_JSON"

git add "$TARGET_PACKAGE_JSON"
git commit -m ":up: Bump up $VERSION"
git tag $VERSION
git push origin master
git push origin master --tags

