using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MediaPlayer
{
    class MusicID3Tag
    {

        public byte[] TAGID = new byte[3]; // 3
        public byte[] Title = new byte[30]; // 30
        public byte[] Artist = new byte[30]; // 30
        public byte[] Album = new byte[30]; // 30
        public byte[] Year = new byte[4]; // 4
        public byte[] Comment = new byte[30]; // 30
        public byte[] Genre = new byte[1]; // 1



       public String filePath="";
       public FileStream fs;

       public MusicID3Tag(String path)
       {
           this.filePath = path;
           this.fs = File.OpenRead(this.filePath);
         
       }


       public String GetMP3Info()
       {
           String path = this.filePath;
           String allInfo = "";
           if (fs.Length >= 128)
           {
               MusicID3Tag tag = new MusicID3Tag(path);
               fs.Seek(-128, SeekOrigin.End);
               fs.Read(tag.TAGID, 0, tag.TAGID.Length);
               fs.Read(tag.Title, 0, tag.Title.Length);
               fs.Read(tag.Artist, 0, tag.Artist.Length);
               fs.Read(tag.Album, 0, tag.Album.Length);
               fs.Read(tag.Year, 0, tag.Year.Length);
               fs.Read(tag.Comment, 0, tag.Comment.Length);
               fs.Read(tag.Genre, 0, tag.Genre.Length);
               string theTAGID = Encoding.Default.GetString(tag.TAGID);


               if (theTAGID.Equals("TAG"))
            {
                string Title = Encoding.Default.GetString(tag.Title);
                string Artist = Encoding.Default.GetString(tag.Artist);
                string Album = Encoding.Default.GetString(tag.Album);
                string Year = Encoding.Default.GetString(tag.Year);
                string Comment = Encoding.Default.GetString(tag.Comment);
                string Genre = Encoding.Default.GetString(tag.Genre);

                Console.WriteLine(Title);
                Console.WriteLine(Artist);
                Console.WriteLine(Album);
                Console.WriteLine(Year);
                Console.WriteLine(Comment);
                Console.WriteLine(Genre);
                Console.WriteLine();

              allInfo="Title: "+Title+System.Environment.NewLine+"Artist: "+Artist +System.Environment.NewLine+ "Album: "+Album+System.Environment.NewLine+ "Year: "+Year+System.Environment.NewLine+"Comment: "+Comment+System.Environment.NewLine+"Genre: "+Genre;
            }
               
          }
           return allInfo;
       }

    }



}
