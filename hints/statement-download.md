# Statement download

## Nudge
The statement download takes a file name. What if the name points somewhere outside the
statements folder?

## Technique
The name is joined to the statements folder without canonicalising the result, so a
traversal can climb out of the folder.

## Near-spoiler
`GET /api/statements?name=` with a name that contains `..` reaches files outside the
statements folder.

Fix: resolve the full path and reject anything that is not inside the statements root,
then return 404. Prove it with a test that a traversal attempt is refused.
