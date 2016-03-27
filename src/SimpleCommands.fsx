#load "Types.fs"
#load "Commands.fs"
#load "Connection.fs"
#load "Minecraft.fs"

open System
open FsMinecraftPi.Types

Minecraft.Connect "localhost"
Minecraft.Chat "Hello from F# (localhost connection)"

Minecraft.Connect Environment.MachineName
Minecraft.Chat (sprintf "Hello from F# (%s connection)" Environment.MachineName)

Minecraft.Block Block.Grass {X = 200.; Y = 100.; Z = 2.}

Minecraft.Blocks Block.Grass {X = 200.; Y = 100.; Z = 1.} {X = 200.; Y = 100.; Z = 1.}

Minecraft.MovePlayer {X = 20.; Y = 10.; Z = 1.}

let pos = Minecraft.PlayerPosition()
{ pos with X = pos.X + 1.} |> Minecraft.ColourBlock Block.Wool BlockColour.Purple

Minecraft.PlayerPosition() |> Minecraft.GetBlock