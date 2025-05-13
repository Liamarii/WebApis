Feature: GetVehiclesByMake

The Vehicles API returns vehicles matching the make requested.

@api @filtering
Scenario Outline: Requesting a make as protobuf returns only vehicles of that make.
	Given a request is made for "<Make>" with the "application/x-protobuf" header
	When the response is received and deserialised from "protobuf"
	Then the response status code is "200"
	And the response only contains vehicles of the expected "<Make>"

Examples:
| Make   |
| Honda  |
| Ford   |
| Nissan |
| Jeep   |
| Tesla  |

@api @filtering
Scenario Outline: Requesting a make as json returns only vehicles of that make.
	Given a request is made for "<Make>" with the "application/json" header
	When the response is received and deserialised from "json"
	Then the response status code is "200"
	And the response only contains vehicles of the expected "<Make>"

Examples:
| Make   |
| Honda  |
| Ford   |
| Nissan |
| Jeep   |
| Tesla  |
