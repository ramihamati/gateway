The reason I chose to use a custom datamapper instead of automapper is that in the mapping list there were a lot
of conversions using between generic content and value objects. 

Automapper did attemtp to manually convert between primaries (string) to value objects(HostObject) even though a map was 
present for the declaring type that specifically and manually instantiated the value object from the primary.
 
After some attempts to fix I decided to give up