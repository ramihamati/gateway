using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.Attributes;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Digitteck.Gateway.TestApi.Api.Aggregates
{
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
}
