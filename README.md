Merge Tool Selector
===================

Most source control systems right now do not let you filter or select
different merge/diff tools based on the file extensions.

This tool is designed to mitigate that problem.


Currently it's almost useless unless you only use Beyond Compare 3
and/or Semantic Merge in their default install locations on Windows.

Consider it pre-alpha, and I'm uploading here as a storage area while
I build it.


###Usage in Git
```
[difftool "mts"]
  cmd = 'C:/Dev/MergeToolSelector/MergeToolSelector/bin/Debug/MergeToolSelector.exe' diff $LOCAL $REMOTE
  trustExitCode = true

[mergetool "mts"]
  cmd = 'C:/Dev/MergeToolSelector/MergeToolSelector/bin/Debug/MergeToolSelector.exe' merge $LOCAL $REMOTE $BASE $MERGED
  trustExitCode = true
```

###Usage in SourceTree
```
External Diff Tool: Custom
Diff Command: <point to exe>
Arguments: diff $LOCAL $REMOTE

External Merge Tool: Custom
Diff Command: <point to exe>
Arguments: merge $LOCAL $REMOTE $BASE $MERGED
```
