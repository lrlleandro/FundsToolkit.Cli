rm -r $args[0];
dotnet new classlib --name $args[0] --language C#;
cd $args[0];
dotnet add package Grpc.Net.Client;
dotnet add package Google.Protobuf;
dotnet add package Grpc.Tools;
mkdir Protos;
copy $args[1] .\Protos;
