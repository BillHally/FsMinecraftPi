module Minecraft
    open System

    open FsMinecraftPi.Connection
    open FsMinecraftPi.Commands
    open FsMinecraftPi.Types
    open FsMinecraftPi

    let private float x =
        try
            float x
        with
        | ex -> raise (Exception(sprintf "Can't convert the string '%s' to a float: %s" x ex.Message, ex))

    let private executeCommand command =
        command |> commandString |> execute

    let private executeCommandAndReturn<'a> command (getReturn: string -> 'a) =
        command |> commandString |> executeAndReturn getReturn

    let private stringToBlock (str : string) =
        enum<Types.Block> (int str)

    let private stringToPlayerPositionInWorld (str : string) =
        if str.StartsWith "Fail: " then
            failwithf "Error: %s" (str.Substring 6)
        else
            let split = str.Split([|','|])
            {
                X = float split.[0]
                Y = float split.[1]
                Z = float split.[2]
            }

    let private stringToResult (str : string) =
        if str = "OK" then
            ()
        else
            failwith str

    let Connect host = connect host
    let ConnectToPort host port = connectToPort host port

    let Chat msg = executeCommand (Say(msg))

    let Block block pos = 
        executeCommand (SetBlock(SimpleBlock(block), pos))
    let Blocks block startPos endPos = 
        executeCommand (SetBlocks(SimpleBlock(block), startPos, endPos))

    let ColourBlock block colour pos = 
        executeCommand (SetBlock(BlockOfColour(block, colour), pos))
    let ColourBlocks block colour startPos endPos = 
        executeCommand (SetBlocks(BlockOfColour(block, colour), startPos, endPos))

    let DataBlock block data pos = 
        executeCommand (SetBlock(BlockWithData(block, data), pos))
    let DataBlocks block data startPos endPos = 
        executeCommand (SetBlocks(BlockWithData(block, data), startPos, endPos))

    let GetBlock pos = 
        executeCommandAndReturn (GetBlock(pos)) stringToBlock

    let MovePlayer pos = executeCommandAndReturn (MovePlayer(pos)) stringToResult

    let PlayerPosition() =
        executeCommandAndReturn (GetPlayerPos) stringToPlayerPositionInWorld
