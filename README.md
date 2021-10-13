A custom gateway created as a Proof Of Concept

Not Production Ready

- It allows defining multiple mapping sources

```json
{
    "RouteSources": [
        "epmovies.json",
        "testingget.json"
    ],
    "Directives": [
        {
            "Directive": "UseRateLimiting",
            "ClientWhitelist": [],
            "Period": "",
            "PeriodTimespan": 1,
            "Limit": 5
        }
    ]
}
```

where 

testingget.json is 

```json
[
    {
        "Endpoint": {
            "Template": "/testingget/all",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/TestingGet/All",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_get_all_persons"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_get_all_persons",
                    "OperationTag": "_returnTag"
                }
            ]
        }
    },
    {
        "Endpoint": {
            "Template": "/testingget/LastName/{lastName}",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/TestingGet/LastName/{lastName}",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_get_all_persons"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_get_all_persons",
                    "OperationTag": "_returnTag"
                }
            ]
        }
    },
    {
        "Endpoint": {
            "Template": "/testingget/Query?firstName={firstName}&lastName={lastName}",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/TestingGet/Query?firstName={firstName}&lastName={lastName}",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_get_all_persons"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_get_all_persons",
                    "OperationTag": "_returnTag"
                }
            ]
        }
    },
    {
        "Endpoint": {
            "Template": "/testingget/Query2?firstName={firstName}",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/TestingGet/Query2?firstName={firstName}",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_get_all_persons"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_get_all_persons",
                    "OperationTag": "_returnTag"
                }
            ]
        }
    }
]


```
**Note: Directives are not implemented**

<h2> Operation Types </h2>

- It allows creating multiple operations for each mapping. Operations can be run sync or async. In async mode
operations are executed in parallel but not without taking into account the eventual dependencies in between them

There are 3 types of operations
  - Call -> executes a get request
  - Return -> internal operation which stores the parameter from one of the previous calls
  - AggregateResponse -> aggregation class defined by the user which can use the captured responses

The OperationTag:
  - Operation = Call : it stores the response with the tag name
  - Operation = Return : retrieves the value and returns it
  - Operation = AggregateResponse: stores the aggregate value in the store with the tag

The ResponseTags:
  - Operation = AggregateResponse: tells the aggregate to use the stored values from the list
  ``` THey are injected with [OperationTag("_call_get_movies")]OperationResult movieResult ```

<h2> Implementing an aggregate response </h2>

- The configuration

```json
 {
        "Endpoint": {
            "Template": "/movies/{movieName}?Rating={rating}",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/movies/{movieName}?Rating={rating}",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_call_get_movies"
                },
                {
                    "Operation": "AggregateResponses",
                    "ResponseTags": [
                        "_call_get_movies"
                    ],
                    "Executor": "Digitteck.Gateway.TestApi.Api, Digitteck.Gateway.TestApi.Api.Aggregates.MovieAggregateResponse",
                    "OperationTag": "_call_get_movies_aggregated"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_call_get_movies_aggregated",
                    "OperationTag": "_returnTag"
                }
            ]
        }
```

- The aggregate class

```
 public class MovieAggregateResponse : IAggregateResponse
    {
        public MovieAggregateResponse([OperationTag("_call_get_movies")]OperationResult movieResult)
        {
            MovieResult = movieResult;
        }

        public OperationResult MovieResult { get; }

        public async Task<OperationResponse> Execute()
        {
            //becase the content is synchronouse, we use TaskRun to make CPU-bound work in a backgroud thread
            return await Task.Run(() =>
            {
                if (!MovieResult.ResponseValue.IsSuccessfull)
                {
                    return MovieResult.ResponseValue;
                }

                MovieModel movie = this.MovieResult.GetResultAs<MovieModel>();
                //altering movdel
                MovieModel newMovie = new MovieModel { MovieName = movie.MovieName + "aggregated", Rating = movie.Rating };

                OperationStringReponse response = new OperationStringReponse(JsonConvert.SerializeObject(newMovie), System.Net.HttpStatusCode.OK);

                return response;
            }).ConfigureAwait(false);
        }
    }
```
The called api
```
    [ApiController]
    [Route("api/Movies")]
    public class MoviesControllers : ControllerBase
    {
        public static List<Movie> movies = new List<Movie>
        {
            new Movie{ MovieName = "AmericanGangster" , Rating = 9.7}
        };

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return movies;
        }

        [HttpGet("{movieName}")]
        public Movie GetMovie(string movieName, double rating)
        {
            return movies.Find(x => x.MovieName == movieName && x.Rating == rating);
        }
    }
```

<h2>Adding the gateway</h2>

`services.AddGW("routeconfig/gwconf.json");`

where `gwconf.json` is 

```json
{
    "RouteSources": [
        "epmovies.json",
        "testingget.json"
    ],
    "Directives": [
        {
            "Directive": "UseRateLimiting",
            "ClientWhitelist": [],
            "Period": "",
            "PeriodTimespan": 1,
            "Limit": 5
        }
    ]
}
```

and `epmovies.json` is

```json
[
    {
        "Endpoint": {
            "Template": "/movies",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/movies",
                    "ServerHost": "localhost",
                    "Scheme": "https",
                    "ServerPort": 44369,
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_call_get_movies"
                },
                {
                    "Operation": "Return",
                    "OperationTag": "_returnTag",
                    "ReturnTag": "_call_get_movies"
                }
            ]
        }
    },
    {
        "Endpoint": {
            "Template": "/movies/{movieName}?Rating={rating}",
            "HttpMethod": "Get"
        },
        "Downstream": {
            "RunAsync": false,
            "Operations": [
                {
                    "Operation": "Call",
                    "Template": "/api/movies/{movieName}?Rating={rating}",
                    "ServerHost": "localhost",
                    "ServerPort": 44369,
                    "Scheme": "https",
                    "HttpMethod": "Get",
                    "Directives": [],
                    "OperationTag": "_call_get_movies"
                },
                {
                    "Operation": "AggregateResponses",
                    "ResponseTags": [
                        "_call_get_movies"
                    ],
                    "Executor": "Digitteck.Gateway.TestApi.Api, Digitteck.Gateway.TestApi.Api.Aggregates.MovieAggregateResponse",
                    "OperationTag": "_call_get_movies_aggregated"
                },
                {
                    "Operation": "Return",
                    "ReturnTag": "_call_get_movies_aggregated",
                    "OperationTag": "_returnTag"
                }
            ]
        }
    }
]

```