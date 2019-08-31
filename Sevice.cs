using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;
using NReco.ImageGenerator;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Discord.Audio;
using System.Messaging;
using Google.Api;
using Google.Apis;
using Google.Cloud.Translation.V2;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using Discord.Rest;
using System.Diagnostics;

namespace ConsoleApp4
{
  public class SC :  ModuleBase<SocketCommandContext>
    {
       
[Command("vc"), Alias("VC")] 
public async Task VC()
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            if (channel == null)
            {
                var DM = await Context.User.GetOrCreateDMChannelAsync();
                await DM.SendMessageAsync("Please Connect to VoiceChannel",true);
            }
            else
            {
                IAudioClient client = await channel.ConnectAsync();
                client.Connected += Con_Connected;
                client.Disconnected += Con_Disconnected;
            
                await client.SetSpeakingAsync(true);


                /*
                  var synth = new SpeechSynthesizer();
                                Directory.CreateDirectory(@"audiofiles");
                                synth.SetOutputToWaveFile(@"audiofiles\\lol.wav");
                                // synth.SetOutputToDefaultAudioDevice();
                                synth.Volume = 100;

                                synth.SpeakAsync("this is a test");

                            */

                var direct =  client.CreatePCMStream(AudioApplication.Music);
      
                
                var ffmpeg = CreateStream(@"C:\audiofiles\lol.wav");
                var stream = ffmpeg.StandardOutput.BaseStream;
                await stream.CopyToAsync(direct);

                await direct.FlushAsync();
                

            }
        }
        private static Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "C:\\ffmpeg-4.2\\ffmpeg-20190826-0821bc4-win64-static\\bin\\ffmpeg.exe",
                Arguments = $@"-i ""{path}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            return Process.Start(ffmpeg);
        }

        private async Task Con_Disconnected(Exception arg)
        {
            var DM = await Context.User.GetOrCreateDMChannelAsync();


var Char=     await DM.SendMessageAsync("Disconnected! Error: "+arg.Message+"Exception: "+arg.Source,true);
   
        }

        private async Task Con_Connected()
        {
            var DM = await Context.User.GetOrCreateDMChannelAsync();
            await DM.SendMessageAsync("Connected!");

     
        }


       
     
        
            
        
    }
}
