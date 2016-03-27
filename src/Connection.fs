namespace FsMinecraftPi

open System
open System.IO
open System.Net.Sockets
open System.Text

module internal Connection =

    let private defaultPort = 4711

    let client : TcpClient option ref = ref None

    let private connectToHostAndPort (host : string) (port : int) =
        match !client with
        | Some x -> x.Close ()
        | None   -> ()

        let x = new TcpClient ()
        x.Connect (host, port)
        client := Some x
        x

    let private connectToDefault () =
        connectToHostAndPort "127.0.0.1" defaultPort

    let private getClient () =
        match client.Value with
        | Some x -> x
        | None   -> connectToDefault ()

    let connect host =
        connectToHostAndPort host defaultPort |> ignore

    let connectToPort host port =
        connectToHostAndPort host port |> ignore

    let execute (command : string) =
        let msg = Encoding.UTF8.GetBytes(sprintf "%s\n" command)
        //printfn "Executing: %s" command
        let client = getClient ()
        client.GetStream().Write(msg, 0, msg.Length)

    let executeAndReturn<'a> (getReturn: string -> 'a) command  =
        execute command

        let client = getClient ()
        let reader = new StreamReader(client.GetStream())
        reader.ReadLine().Replace("|", Environment.NewLine) |> getReturn
