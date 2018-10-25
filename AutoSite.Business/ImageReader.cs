using AutoSite.Core.Models;
using AutoSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Business
{
    public class ImageReader : IImageReader
    {
        private readonly ComputerVisionClient client;
        private readonly ApiConsumer consumer;

        public ImageReader(ComputerVisionClient visionClient, HttpClient httpClient)
        {
            client = visionClient;
            consumer = new ApiConsumer(httpClient);
        }

        public async Task<string[]> GetTextAsync(IFormFile image, bool isTable = false)
        {
            var operation = await client.RecognizeTextInStreamAsync(image.OpenReadStream(), TextRecognitionMode.Printed);
            TextOperationResult result;
            do
            {
                await Task.Delay(500);
                result = await consumer.GetAsync<TextOperationResult>(operation.OperationLocation);
            } while (result.Status == TextOperationStatusCodes.NotStarted
                        || result.Status == TextOperationStatusCodes.Running);
            
            return ComputeLines(result.RecognitionResult.Lines.Select(l => new LineContext {
                Text = l.Text,
                BoundingBox = l.BoundingBox.ToArray()
            }).ToArray(), isTable);
        }

        string[] ComputeLines(LineContext[] lines, bool isTable)
        {

            var dims = lines.Select(l => l.BoundingBox.ToArray()).ToArray();

            var result = new List<LineContext>();
            var used = new List<int>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (!used.Contains(i))
                {
                    var line = lines[i];
                    var dim = line.BoundingBox;
                    bool matched = false;
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        if (!used.Contains(j))
                        {
                            var line2 = lines[j];
                             var dim2 = line2.BoundingBox;

                            // check inbetween using (a - c) * (b - c) is negitive or 0
                            if (((dim[3] - dim2[1]) * (dim[5] - dim2[1]) <= 0)
                                || ((dim[3] - dim2[7]) * (dim[5] - dim2[7]) <= 0))
                            {
                                // in the same line
                                matched = true;
                                if (dim[0] < dim2[0])
                                {
                                    // first is left of second
                                    if (dim[2] > dim2[0])
                                    {
                                        // second overlaps first
                                        result.Add(OverlapIndex(line.Text, line2.Text, dim, dim2));
                                    }
                                    else
                                    {
                                        if (isTable) matched = false;
                                        else result.Add(MergeBounds(line.Text + " " + line2.Text, dim, dim2));
                                    }
                                }
                                else
                                {
                                    // first is right of second
                                    if (dim2[2] > dim[0])
                                    {
                                        // first overlaps second
                                        result.Add(OverlapIndex(line2.Text, line.Text, dim2, dim));
                                    }
                                    else
                                    {
                                        if (isTable) matched = false;
                                        else result.Add(MergeBounds(line2.Text + " " + line.Text, dim2, dim));
                                    }
                                }
                                if (matched)
                                {
                                    used.Add(j);
                                    break;
                                }
                            }
                        }
                    }
                    if (!matched) result.Add(line);
                }
            }
            if (isTable)
            {
                // vertical checks
                lines = result.ToArray();
                result = new List<LineContext>();
                used = new List<int>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!used.Contains(i))
                    {
                        var line1 = lines[i];
                        var dim1 = line1.BoundingBox;
                        bool matched = false;
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            if (!used.Contains(i))
                            {
                                var line2 = lines[j];
                                var dim2 = line2.BoundingBox;

                                if ((dim1[6] - dim2[0]) * (dim1[4] - dim2[0]) <= 0 ||
                                    (dim1[6] - dim2[2]) * (dim1[4] - dim2[2]) <= 0 ||
                                    (dim2[6] - dim1[0]) * (dim2[4] - dim1[0]) <= 0 ||
                                    (dim2[6] - dim1[2]) * (dim2[4] - dim1[2]) <= 0)
                                {
                                    matched = true;
                                    result.Add(MergeBounds(line1.Text + ":|:" + line2.Text, dim1, dim2));
                                    used.Add(j);
                                    break;
                                }
                            }
                        }
                        if (!matched) result.Add(line1);
                    }
                }
            }
            return result.Select(r => r.Text).ToArray();
        }

        string OverlapIndex(string s1, string s2)
        {
            int limit = Math.Min(s1.Length, s2.Length) - 1;
            int i;
            for (i = 0; i < limit && 
                        s1.Substring(s1.Length - i - 1) != s2.Substring(0, i + 1); i++);
            if (i == limit) return s1 + s2;
            return s1.Substring(0, s1.Length - i - 1) + s2;
        }

        LineContext OverlapIndex(string s1, string s2, int[] dim1, int[] dim2)
            => MergeBounds(OverlapIndex(s1, s2), dim1, dim2);

        LineContext MergeBounds(string text, int[] dim1, int[] dim2)
                => new LineContext
                {
                    Text = text,
                    BoundingBox = new int[] {
                        dim1[0], dim1[1],
                        dim2[2], dim2[3], dim2[4], dim2[5],
                        dim1[6], dim1[7]
                    }
                };

        [DebuggerDisplay("Text = {Text}")]
        class LineContext
        {
            public string Text { get; set; }

            public int[] BoundingBox { get; set; }

        }

    }
}
