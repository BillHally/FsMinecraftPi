namespace FsMinecraftPi

open System
open System.IO
open System.Net.Sockets
open System.Text

module internal Connection =
    let client = new TcpClient()
    client.Connect("127.0.0.1", 4711)

    let execute (command : string) =
        let msg = Encoding.UTF8.GetBytes(sprintf "%s\n" command)
        printfn "Executing: %s" command
        client.GetStream().Write(msg, 0, msg.Length)

    let executeAndReturn<'a> (getReturn: string -> 'a) command  =
        execute command
        let reader = new StreamReader(client.GetStream())
        reader.ReadLine().Replace("|", Environment.NewLine) |> getReturn
