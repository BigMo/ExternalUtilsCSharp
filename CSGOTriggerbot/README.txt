	Note:
There are two approaches on how to read data from the/any game (externally):

	Approach #1: Read structures
Reading plain structs is great since they are read from memory only ONCE each frame: The value of each field is read once.
Also, field-offsets are static: Signature-scanning won't help us since we can't change the offsets of fields within the structs.

	Approach #2: Read single fields
Since structures don't support any kind of inheritance, there's a lot of redundant code (getting the EntityID for example).
By implementing classes which inherit from base-classes, one can wipe out redundant code: Resulting in more RPM-calls.

The folder "CSGOStructs" implements approach #1, "CSGOClasses" implements approach #2.