namespace FsUnit

[<AutoOpen>]
module Polyfills =
    let shouldEqual<'T> (expected: 'T) (actual: 'T) =
        expected |> should equal actual

    let shouldFail<'E> (f: unit -> unit) =
        should throw typeof<'E> f
