# Mediator

The mediator pattern, in my opinion, tries to encapsulate the flow of the data, so how the data is processed.
The mediator handles all the needed communication between the different objects and services, until we get the result we want.
As a full stack developer it reminds me of RX pattern, where the pipe is the mediator.
The pipe defines for me all interaction of the object with other objects and services and return the result in the end.

This is as well how I used the Mediator in this project.

## How I used the Mediator

All mediator related object are in the layer UseCases.
Each of this object defines a action, which can be executed.
In the case of the HouseService, we have the following actions:

- AddHouse
- GetHouseByID
- SearchHouses

Each of this action have their own handler, which is responsible for the execution of the action.

In general, a handler should not call another handler, as this would break the single responsibility principle.
It should only interact with services defined in the core Layer.
This services can be implemented either by the core layer or by the infrastructure layer.

### What to be aware of the handler

The mediator pattern defines a pipeline, where extra layer can be added before and after an action is handled.
This can help to decouple code further.