#!/bin/bash
# Sorts json files by Trickle conventions documented in Wiki.

jq -S ".|=sort_by(.dependencyTrickleId, .dependentTrickleId)" Dependent.json > Dependent.tmp
mv Dependent.tmp Dependent.json

jq -S ".|=sort_by(.id)" Trickle.json > Trickle.tmp
mv Trickle.tmp Trickle.json

jq -S ".|=sort_by(.TrickleId, .languageId)" TrickleLanguage.json > TrickleLanguage.tmp
mv TrickleLanguage.tmp TrickleLanguage.json

jq -S ".|=sort_by(.TrickleId, .maintainerId)" TrickleMaintainer.json > TrickleMaintainer.tmp
mv TrickleMaintainer.tmp TrickleMaintainer.json

jq -S ".|=sort_by(.TrickleId, .syntaxId)" TrickleSyntax.json > TrickleSyntax.tmp
mv TrickleSyntax.tmp TrickleSyntax.json

jq -S ".|=sort_by(.TrickleId, .tagId)" TrickleTag.json > TrickleTag.tmp
mv TrickleTag.tmp TrickleTag.json

jq -S ".|=sort_by(.id)" TrickleViewUrl.json > TrickleViewUrl.tmp
mv TrickleViewUrl.tmp TrickleViewUrl.json

jq -S ".|=sort_by(.upstreamTrickleId, .forkTrickleId)" Fork.json > Fork.tmp
mv Fork.tmp Fork.json

jq -S ".|=sort_by(.iso6391)" Language.json > Language.tmp
mv Language.tmp Language.json

jq -S ".|=sort_by(.id)" License.json > License.tmp
mv License.tmp License.json

jq -S ".|=sort_by(.id)" Maintainer.json > Maintainer.tmp
mv Maintainer.tmp Maintainer.json

jq -S ".|=sort_by(.includedInTrickleId, .includesTrickleId)" Merge.json > Merge.tmp
mv Merge.tmp Merge.json

jq -S ".|=sort_by(.id)" Software.json > Software.tmp
mv Software.tmp Software.json

jq -S ".|=sort_by(.softwareId, .syntaxId)" SoftwareSyntax.json > SoftwareSyntax.tmp
mv SoftwareSyntax.tmp SoftwareSyntax.json

jq -S ".|=sort_by(.id)" Syntax.json > Syntax.tmp
mv Syntax.tmp Syntax.json

jq -S ".|=sort_by(.id)" Tag.json > Tag.tmp
mv Tag.tmp Tag.json
