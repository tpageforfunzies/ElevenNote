using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ElevenNote.Models;
using ElevenNoteMobileApp.Contracts;
using ElevenNoteMobileApp.Models;
using Newtonsoft.Json;

namespace ElevenNoteMobileApp.Services
{
    internal class WebNoteService : INoteService
    {
        public static string BearerToken { get; set; }
        private const string _apiUrl = "https://auri-efa-net301-nov2017-elevennote-api.azurewebsites.net";

        /// <summary>
        /// Attempts to log the user in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Build API URL.
                    var url = $"{_apiUrl}/token";

                    // Construct the request.
                    var requestString = $"grant_type=password&username={HttpUtility.UrlEncode(username.Trim())}&password={HttpUtility.UrlEncode(password.Trim())}";
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "www-form-urlencoded; charset=utf-8");

                    // Get the response and set the bearer token if one was returned.
                    var result = await client.PostAsync(url, new StringContent(requestString));
                    if (!result.IsSuccessStatusCode)
                    {
                        BearerToken = null;
                        return false;
                    }

                    var stringResponseFromServer = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<OauthBearerTokenResponse>(stringResponseFromServer);
                    if (response == null) return false;
                    BearerToken = response.access_token; // can't use Deserialize<dynamic> or iOS will go boom
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a list of all the user's notes.
        /// </summary>
        /// <returns></returns>
        public async Task<List<NoteListItem>> GetAll()
        {
            if (string.IsNullOrWhiteSpace(BearerToken)) throw new UnauthorizedAccessException("Bearer token not initialized. Aborting.");

            using (var client = new HttpClient())
            {
                // Build API URL.
                var url = $"{_apiUrl}/api/notes";

                // Construct the request.
                // Options:
                //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {BearerToken}"); OR
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                // Make the call and get the result.
                var result = await client.GetAsync(url);

                // If the call failed, return an empty list.
                if (!result.IsSuccessStatusCode) return new List<NoteListItem>();

                // Otherwise, deserialize the result and return for use.
                var notes = JsonConvert.DeserializeObject<List<NoteListItem>>(await result.Content.ReadAsStringAsync());
                return notes;
            }
        }

        /// <summary>
        /// Gets note details by ID.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public async Task<NoteDetail> GetById(int noteId)
        {
            if (string.IsNullOrWhiteSpace(BearerToken)) throw new UnauthorizedAccessException("Bearer token not initialized. Aborting.");

            using (var client = new HttpClient())
            {
                // Build API URL.
                var url = $"{_apiUrl}/api/notes/{ noteId }"; // endpoint uses "id" as default, so we don't have to create a querystring with "id="

                // Construct the request.
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                // Make the call and get the result.
                var result = await client.GetAsync(url);

                // If the call failed, return a null note object.
                if (!result.IsSuccessStatusCode) return null;

                // Otherwise, deserialize the result and return the note details object for use.
                var note = JsonConvert.DeserializeObject<NoteDetail>(await result.Content.ReadAsStringAsync());
                return note;
            }

        }

        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task<bool> AddNew(NoteCreate note)
        {
            if (string.IsNullOrWhiteSpace(BearerToken)) throw new UnauthorizedAccessException("Bearer token not initialized. Aborting.");

            using (var client = new HttpClient())
            {
                // Build API URL.
                var url = $"{_apiUrl}/api/notes";

                // Construct the request.
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                // Create the JSON version of the note object. JSON is a string we'll send to the server.
                var json = JsonConvert.SerializeObject(note);

                // Make the call and get the result.
                var result = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")); // we have to specify we're sending JSON

                // If the call failed, return an empty list.
                return result.IsSuccessStatusCode;

            }
        }

        /// <summary>
        /// Updates the passed note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task<bool> Update(NoteEdit note)
        {
            if (string.IsNullOrWhiteSpace(BearerToken)) throw new UnauthorizedAccessException("Bearer token not initialized. Aborting.");

            using (var client = new HttpClient())
            {
                var url = $"{_apiUrl}/api/notes";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                var json = JsonConvert.SerializeObject(note);
                var result = await client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json")); // we have to specify we're sending JSON

                return result.IsSuccessStatusCode;

            }
        }

        /// <summary>
        /// Deletes the passed note by ID.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(int noteId)
        {
            if (string.IsNullOrWhiteSpace(BearerToken)) throw new UnauthorizedAccessException("Bearer token not initialized. Aborting.");

            using (var client = new HttpClient())
            {
                var url = $"{_apiUrl}/api/notes/{noteId}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                var result = await client.DeleteAsync(url); // DELETE is similar to a GET call - it has no body

                return result.IsSuccessStatusCode;
            }
        }
    }
}
