using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Json;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AnastasiaHueApp.Tests.Util.Json;

[TestClass]
[TestSubject(typeof(JsonRegistry))]
public class JsonRegistryTest
{
    #region TestClasses

    private class Passport()
    {
        public required string FullName { get; init; }
        public required string Nationality { get; init; }
        public int Age { get; init; }
    }

    #endregion

    [TestMethod]
    public void Parse_Passport_MatchingValues()
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());
        registry.Register<Passport>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return new Passport
            {
                FullName = doc.RootElement.GetProperty("FullName").GetString()!,
                Nationality = doc.RootElement.GetProperty("Nationality").GetString()!,
                Age = doc.RootElement.GetProperty("Age").GetInt32(),
            };
        });

        // Act.
        var passport = registry.Parse<Passport>("""
                                                {
                                                  "FullName": "John Doe",
                                                  "Nationality": "American",
                                                  "Age": 34
                                                }
                                                """);

        // Assert.
        passport.FullName.Should().Be("John Doe");
        passport.Nationality.Should().Be("American");
        passport.Age.Should().Be(34);
    }

    [TestMethod]
    public void Parse_Passport_InvalidString_ReturnsNull()
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());
        registry.Register<Passport>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return new Passport
            {
                FullName = doc.RootElement.GetProperty("FullName").GetString()!,
                Nationality = doc.RootElement.GetProperty("Nationality").GetString()!,
                Age = doc.RootElement.GetProperty("Age").GetInt32(),
            };
        });

        // Act.
        var passport = registry.Parse<Passport>("""
                                                {
                                                  "FavouriteFood": "Pizza",
                                                  "BestFootballer": "Cristiano Ronaldo",
                                                  "FavouriteChildhoodGame": "Minecraft"
                                                }
                                                """);

        // Assert.
        passport.Should().BeNull("the json string was incorrect, thus it is not of this type of object");
    }

    #region HueParsing

    // SEE: https://developers.meethue.com/develop/hue-api/error-messages/
    // Emulator json.
    [TestMethod]
    [DataRow("""
             [
                 {
                     "error": {
                         "type": "2",
                         "address": "",
                         "description": "body contains invalid JSON"
                     }
                 }
             ]
             """, 2)]
    [DataRow("""
             [
                 {
                     "error": {
                         "type": "101",
                         "address": "",
                         "description": "link button has not been pressed"
                     }
                 }
             ]
             """, 101)]
    [DataRow("""
             [
               {
                 "success": {
                   "/lights/1/state/sat": 0
                 }
               },
               {
                 "success": {
                   "/lights/1/state/bri": 10
                 }
               },
               {
                 "error": {
                   "address": "/lights/1/state/hue",
                   "description": "invalid value, -1 , for parameter, hue",
                   "type": 7
                 }
               },
               {
                 "success": {
                   "/lights/1/state/on": true
                 }
               }
             ]
             """, 7)]
    [DataRow("""
             [
               {
                 "error": {
                   "address": "/lights/1/state/sat",
                   "description": "invalid value, -1 , for parameter, sat",
                   "type": 7
                 }
               },
               {
                 "error": {
                   "address": "/lights/1/state/bri",
                   "description": "invalid value, -1 , for parameter, bri",
                   "type": 7
                 }
               },
               {
                 "error": {
                   "address": "/lights/1/state/hue",
                   "description": "invalid value, -1 , for parameter, hue",
                   "type": 7
                 }
               },
               {
                 "success": {
                   "/lights/1/state/on": true
                 }
               }
             ]
             """, 7)]
    // Real hue json.
    [DataRow("""
             [
               {
                 "error": {
                   "type": 7,
                   "address": "/lights/10/state/bri",
                   "description": "invalid value, -1 }, for parameter, bri"
                 }
               }
             ]
             """, 7)]
    [DataRow("""
             [
               {
                 "error": {
                   "type": 7,
                   "address": "/lights/10/state/bri",
                   "description": "invalid value, -1,, for parameter, bri"
                 }
               },
               {
                 "error": {
                   "type": 7,
                   "address": "/lights/10/state/hue",
                   "description": "invalid value, -1,, for parameter, hue"
                 }
               },
               {
                 "error": {
                   "type": 201,
                   "address": "/lights/10/state/sat",
                   "description": "parameter, sat, is not modifiable. Device is set to off."
                 }
               }
             ]
             """, 7)]
    [DataRow("""
             [
               {
                 "error": {
                   "type": 201,
                   "address": "/lights/10/state/sat",
                   "description": "parameter, sat, is not modifiable. Device is set to off."
                 }
               }
             ]
             """, 201)]
    [DataRow("""
             [
               {
                 "error": {
                   "type": 7,
                   "address": "/lights/11/state/sat",
                   "description": "invalid value, -1 }, for parameter, sat"
                 }
               },
               {
                 "success": {
                   "/lights/11/state/on": true
                 }
               },
               {
                 "success": {
                   "/lights/11/state/hue": 2
                 }
               },
               {
                 "success": {
                   "/lights/11/state/bri": 5
                 }
               }
             ]
             """, 7)]
    [DataRow("""
             [
               {
                 "error": {
                   "type": 101,
                   "address": "/",
                   "description": "link button not pressed"
                 }
               }
             ]
             """, 101)]
    public void Parse_ErrorMessage_CanParse_NotNullAndTypeEquals([StringSyntax(StringSyntaxAttribute.Json)] string json,
        int errorType)
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());

        // Act.
        var response = registry.Parse<ErrorResponse>(json);

        // Assert.
        response.Should().NotBeNull();
        response!.Type.Should().Be($"{errorType}");
    }

    [TestMethod]
    // Emulator json.
    [DataRow("""
             [
               {
                 "success": {
                   "username": "5729c8e92cb6812aaa58471ce6b36cb"
                 }
               }
             ]
             """, "5729c8e92cb6812aaa58471ce6b36cb")]
    // Real hue json.
    [DataRow("""
             [
               {
                 "success": {
                   "username": "hdOIRli7m4e8ojJABvWxeoY-YznaMh5c7wBG5zmj"
                 }
               }
             ]
             """)]
    public void Parse_UsernameResponse_CanParse_NotNullAndUsernameEquals(
        [StringSyntax(StringSyntaxAttribute.Json)]
        string json, string username)
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());

        // Act.
        var response = registry.Parse<UsernameResponse>(json);

        // Assert.
        response.Should().NotBeNull();
        response!.Username.Should().Be(username);
    }

    // Emulator json.
    [TestMethod]
    [DataRow("""
             {
               "state": {
                 "on": true,
                 "bri": 225,
                 "hue": 54612,
                 "sat": 205,
                 "xy": [
                   0,
                   0
                 ],
                 "ct": 0,
                 "alert": "lselect",
                 "effect": "colorloop",
                 "colormode": "hs",
                 "reachable": true
               },
               "type": "Extended color light",
               "name": "Hue Lamp 1",
               "modelid": "LCT001",
               "swversion": "65003148",
               "uniqueid": "00:17:88:01:00:d4:12:08-0a",
               "pointsymbol": {
                 "1": "none",
                 "2": "none",
                 "3": "none",
                 "4": "none",
                 "5": "none",
                 "6": "none",
                 "7": "none",
                 "8": "none"
               }
             }
             """)]
    // Hue light json.
    [DataRow("""
             {
               "state": {
                 "on": false,
                 "bri": 254,
                 "hue": 0,
                 "sat": 0,
                 "effect": "none",
                 "xy": [
                   0.4573,
                   0.4100
                 ],
                 "ct": 367,
                 "alert": "none",
                 "colormode": "xy",
                 "reachable": false
               },
               "type": "Extended color light",
               "name": "i",
               "modelid": "LCT001",
               "manufacturername": "Philips",
               "uniqueid": "00:17:88:01:00:dc:04:19-0b",
               "swversion": "5.127.1.26420"
             }
             """)]
    public void Parse_HueLight_CanParse_NotNullAndEquals([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());
        var expectedLight = new HueLight
        {
            Id = int.MinValue, // NOTE: In reality one. However, we do not parse ids from single lights since they are embedded in the requests!
            ModelId = "LCT001",
            Name = "Hue Lamp 1",
            SwVersion = "65003148",
            UniqueId = "00:17:88:01:00:d4:12:08-0a",
            Type = "Extended color light",
            PointSymbol = new PointSymbol
            {
                Symbol1 = "none",
                Symbol2 = "none",
                Symbol3 = "none",
                Symbol4 = "none",
                Symbol5 = "none",
                Symbol6 = "none",
                Symbol7 = "none",
                Symbol8 = "none"
            },
            State = new HueLightState
            {
                On = true,
                Brightness = 225,
                Hue = 54612,
                Saturation = 205,
                XyPoint = new Point(0, 0),
                Ct = 0,
                Alert = HueAlert.LSelect,
                Effect = HueEffect.ColorLoop,
                ColorMode = HueColorMode.Hs,
                Reachable = true
            }
        };

        // Act.
        var actualLight = registry.Parse<HueLight>(json);

        // Assert.
        actualLight.Should().BeEquivalentTo(expectedLight, options =>
            options
                .WithStrictOrdering()
                .IncludingAllDeclaredProperties()
                .Excluding(l => l.Id)); // IDS have to be set manually for single light parsing. Check JsonRegistry class documentation.
    }

    // Emulator json.
    [TestMethod]
    [DataRow("""
             {
               "1": {
                 "modelid": "LCT001",
                 "name": "Hue Lamp 1",
                 "swversion": "65003148",
                 "state": {
                   "xy": [
                     0,
                     0
                   ],
                   "ct": 0,
                   "alert": "lselect",
                   "sat": 205,
                   "effect": "colorloop",
                   "bri": 225,
                   "hue": 54612,
                   "colormode": "hs",
                   "reachable": true,
                   "on": true
                 },
                 "type": "Extended color light",
                 "pointsymbol": {
                   "1": "none",
                   "2": "none",
                   "3": "none",
                   "4": "none",
                   "5": "none",
                   "6": "none",
                   "7": "none",
                   "8": "none"
                 },
                 "uniqueid": "00:17:88:01:00:d4:12:08-0a"
               },
               "2": {
                 "modelid": "LCT001",
                 "name": "Hue Lamp 2",
                 "swversion": "65003148",
                 "state": {
                   "xy": [
                     0.346,
                     0.3568
                   ],
                   "ct": 201,
                   "alert": "none",
                   "sat": 144,
                   "effect": "none",
                   "bri": 254,
                   "hue": 23536,
                   "colormode": "hs",
                   "reachable": true,
                   "on": true
                 },
                 "type": "Extended color light",
                 "pointsymbol": {
                   "1": "none",
                   "2": "none",
                   "3": "none",
                   "4": "none",
                   "5": "none",
                   "6": "none",
                   "7": "none",
                   "8": "none"
                 },
                 "uniqueid": "00:17:88:01:00:d4:12:08-0b"
               },
               "3": {
                 "modelid": "LCT001",
                 "name": "Hue Lamp 3",
                 "swversion": "65003148",
                 "state": {
                   "xy": [
                     0.346,
                     0.3568
                   ],
                   "ct": 201,
                   "alert": "none",
                   "sat": 254,
                   "effect": "none",
                   "bri": 254,
                   "hue": 65136,
                   "colormode": "hs",
                   "reachable": true,
                   "on": true
                 },
                 "type": "Extended color light",
                 "pointsymbol": {
                   "1": "none",
                   "2": "none",
                   "3": "none",
                   "4": "none",
                   "5": "none",
                   "6": "none",
                   "7": "none",
                   "8": "none"
                 },
                 "uniqueid": "00:17:88:01:00:d4:12:08-0c"
               }
             }
             """)]
    // Real hue json.
    [DataRow("""
            {
              "10": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 0,
                  "sat": 0,
                  "effect": "none",
                  "xy": [
                    0.4573,
                    0.4100
                  ],
                  "ct": 367,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": false
                },
                "type": "Extended color light",
                "name": "i",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:dc:04:19-0b",
                "swversion": "5.127.1.26420"
              },
              "11": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 14988,
                  "sat": 141,
                  "effect": "none",
                  "xy": [
                    0.4575,
                    0.4101
                  ],
                  "ct": 366,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "hate",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:f6:78:db-0b",
                "swversion": "5.127.1.26420"
              },
              "12": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 14988,
                  "sat": 141,
                  "effect": "none",
                  "xy": [
                    0.4575,
                    0.4101
                  ],
                  "ct": 366,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "all",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:dc:07:70-0b",
                "swversion": "5.127.1.26420"
              },
              "14": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 14988,
                  "sat": 141,
                  "effect": "none",
                  "xy": [
                    0.4575,
                    0.4101
                  ],
                  "ct": 366,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "the",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:f6:79:a4-0b",
                "swversion": "5.105.0.21536"
              },
              "15": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 14988,
                  "sat": 141,
                  "effect": "none",
                  "xy": [
                    0.4575,
                    0.4101
                  ],
                  "ct": 366,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "fucking",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:de:f0:54-0b",
                "swversion": "5.127.1.26420"
              },
              "16": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 32929,
                  "sat": 220,
                  "effect": "none",
                  "xy": [
                    0.3337,
                    0.3580
                  ],
                  "ct": 181,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "stupid",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:f6:79:8a-0b",
                "swversion": "5.127.1.26420"
              },
              "17": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 32636,
                  "sat": 218,
                  "effect": "none",
                  "xy": [
                    0.3370,
                    0.3638
                  ],
                  "ct": 187,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "annoying",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:f6:79:2a-0b",
                "swversion": "5.127.1.26420"
              },
              "18": {
                "state": {
                  "on": false,
                  "bri": 254,
                  "hue": 34391,
                  "sat": 226,
                  "effect": "none",
                  "xy": [
                    0.3178,
                    0.3285
                  ],
                  "ct": 159,
                  "alert": "none",
                  "colormode": "xy",
                  "reachable": true
                },
                "type": "Extended color light",
                "name": "n......",
                "modelid": "LCT001",
                "manufacturername": "Philips",
                "uniqueid": "00:17:88:01:00:f6:79:a0-0b",
                "swversion": "5.127.1.26420"
              }
            }
            """)]
    public void Parse_HueLightList_CanParse_Light1NotNullAndEquals([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        // Arrange.
        var registry = new JsonRegistry(Mock.Of<ILogger<JsonRegistry>>());
        var expectedLight = new HueLight
        {
            Id = 1,
            ModelId = "LCT001",
            Name = "Hue Lamp 1",
            SwVersion = "65003148",
            UniqueId = "00:17:88:01:00:d4:12:08-0a",
            Type = "Extended color light",
            PointSymbol = new PointSymbol
            {
                Symbol1 = "none",
                Symbol2 = "none",
                Symbol3 = "none",
                Symbol4 = "none",
                Symbol5 = "none",
                Symbol6 = "none",
                Symbol7 = "none",
                Symbol8 = "none"
            },
            State = new HueLightState
            {
                On = true,
                Brightness = 225,
                Hue = 54612,
                Saturation = 205,
                XyPoint = new Point(0, 0),
                Ct = 0,
                Alert = HueAlert.LSelect,
                Effect = HueEffect.ColorLoop,
                ColorMode = HueColorMode.Hs,
                Reachable = true
            }
        };

        // Act.
        var actualLight = registry.Parse<List<HueLight>>(json);

        // Assert.
        actualLight.Should().NotBeNull();
        actualLight!.First().Should().NotBeNull();
        actualLight!.First().Should().BeEquivalentTo(expectedLight, options =>
            options
                .WithStrictOrdering()
                .IncludingAllDeclaredProperties());
    }

    #endregion
}