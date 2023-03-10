cd $args[0];
dir;
dotnet publish -c release -o $args[1];
