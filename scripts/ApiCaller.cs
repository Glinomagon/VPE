using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Godot;

namespace VPE.scripts;

public class ApiCaller {
  // have to use full name for HttpClient or else it confuses it with Godot's HttpClient
  private static readonly System.Net.Http.HttpClient httpClient = new();

  // For downloading files
  public static async Task<byte[]> GetByteArrayAsync(string url, Dictionary<string, string> headers = null) {
    try {
      HttpRequestMessage request = new(HttpMethod.Get, url);
      // add headers
      if (headers != null) {
        foreach (var header in headers) {
          request.Headers.Add(header.Key, header.Value);
        }
      }

      HttpResponseMessage response = await httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsByteArrayAsync();
    } catch (Exception e) { // catch everything
      GD.PrintErr($"Download on {url} failed! {e.Message}");
      return null;
    }
  }
}