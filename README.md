<h1>Councilor Levels</h1>

Creates the notion of a Councilor Level. This level represents the broad expertise of the Councilor
and increments every time the player or AI applies any augmentation (yes, this includes Cybernetics).

This value is not currently exposed to the player but can be accessed using the CouncilorLevelManager,
which is a wrapper around a Register of councilors & their levels.

<h2>Currently supported:</h2>

Adding a new councilor to the Registry or incrementing the level of an existing councilor via
CouncilorLevelManagerExternalMethods.AddOrIncrementCouncilorLevel

Decrementing a councilor's level via CouncilorLevelManagerExternalMethods.DecrementCouncilorLevel

Removing a councilor from the register via CouncilorLevelManagerExternalMethods.RemoveCouncilorLevel

Retrieving a councilor level from the register via CouncilorLevelManagerExternalMethods.GetCouncilorLevel

<h2>Integration notes:</h2>

CouncilorLevels _only uses Postfixes_ and therefore should be compatible with any mod that doesn't break
the game. 

CouncilorLevels will automatically add each councilor to the register when they are initialized via
`InitWithTemplate`. Additionally, it will automatically increment level when `ApplyAugmentation` is called.
Finally, when a councilor is killed they are automatically cleared from the registry.

This mod is NOT compatible with existing saves as there is no way to determine what level each councilor
should have by their combination of attributes and traits.

<h2>Changelog:</h2>

October 14 2022 - Initial Upload