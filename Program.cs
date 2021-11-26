using System;
using System.IO;
using System.Collections.Generic;

namespace foldersizecalculator
{
    /*
    dotnet publish --output "t:/programs/foldersizecalculator" --runtime win-x64 --configuration Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
    */
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculating..");
            var folderPath = args[0];
            var files = Directory.GetFiles(folderPath);
            var dirs = Directory.GetDirectories(folderPath);
            var fileFolderDatas = new List<FileFolderData>();
            for(int i=0;i<files.Length;i++){
                var fInfo = new FileInfo(files[i]);
                fileFolderDatas.Add(new FileFolderData(){
                    Name=fInfo.Name,
                    Size=fInfo.Length,
                    IsFolder=false
                });
            }
            for(int i=0;i<dirs.Length;i++){
                try{
                    fileFolderDatas.Add(GetFolderData(dirs[i]));
                }catch{
                    Console.WriteLine($"Can not read folder ${dirs[i]}");
                };
            }
            var fileFolderDatasArr = fileFolderDatas.ToArray();
            heapSort(fileFolderDatasArr);
            ConsoleColor originalColor = Console.ForegroundColor;

            for(int i=0; i<fileFolderDatasArr.Length;i++){
                if(fileFolderDatasArr[i].IsFolder && Console.ForegroundColor!=ConsoleColor.Green){
                    Console.ForegroundColor=ConsoleColor.Green;
                }
                if(!fileFolderDatasArr[i].IsFolder && Console.ForegroundColor!=ConsoleColor.Blue){
                    Console.ForegroundColor=ConsoleColor.Blue;
                }
                
                var fileMB = Math.Round((double)fileFolderDatasArr[i].Size / (1024*1024),1); 
                Console.WriteLine($"{fileFolderDatasArr[i].Name}: {fileMB}MB");
            }
            Console.ForegroundColor = originalColor;

        }

        static FileFolderData GetFolderData(string folderPath){
            var files = Directory.GetFiles(folderPath);
            var dirs = Directory.GetDirectories(folderPath);
            double size = 0;
            for(int i=0;i<files.Length;i++){
                var fInfo = new FileInfo(files[i]);
                size += fInfo.Length;
            }
            if(dirs.Length==0){
                return new FileFolderData(){
                    Name=new DirectoryInfo(folderPath).Name,
                    Size= size,
                    IsFolder=true
                };
            }else{
                foreach (var dir in dirs)
                {
                    size += GetFolderData(dir).Size;
                }
                 return new FileFolderData(){
                    Name=new DirectoryInfo(folderPath).Name,
                    Size= size,
                    IsFolder=true
                };
            }

        }
        
        class FileFolderData{
            public string Name {get;set;}
            public double Size {get;set;}
            public bool  IsFolder {get;set;}
        }
        static void heapSort(FileFolderData[] arr) {
         var n = arr.Length;
         for (int i = n / 2 - 1; i >= 0; i--)
         heapify(arr, n, i);
         for (int i = n-1; i>=0; i--) {
            FileFolderData temp = arr[0];
            arr[0] = arr[i];
            arr[i] = temp;
            heapify(arr, i, 0);
        }

        static void heapify(FileFolderData[] arr, int n, int i) {
            int largest = i;
            int left = 2*i + 1;
            int right = 2*i + 2;
            if (left < n && arr[left].Size > arr[largest].Size)
            largest = left;
            if (right < n && arr[right].Size > arr[largest].Size)
            largest = right;
            if (largest != i) {
                FileFolderData swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;
                heapify(arr, n, largest);
            }
        }
      }

    }

}

