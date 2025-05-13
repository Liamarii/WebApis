Feature: GetVehiclesByMake

The Vehicles API returns correct data when filtering vehicles by make.

@api @filtering
Scenario Outline: Requesting a valid make returns a fitered response.
	Given a request is made for "<Make>"
	When the response is received
	Then the response status code is "200"
	And the response contains a list of vehicles
	And all vehicles in the response have the make "<Make>"

Examples:
| Make   |
| Honda  |
| Ford   |
| Nissan |
| Jeep   |
| Tesla  |