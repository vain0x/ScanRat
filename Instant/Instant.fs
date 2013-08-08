﻿module Instant

open InstantCombinators
open System.Collections.Generic

let inline (~~) (str:string) = pStr str

let oneOf = pOneOf

type ParsingSuccess<'v> = { consumed: int; value: 'v } 
type ParsingFailure = { index: int; expectations: string list}

type ParsingResult<'v> =
    | Success of ParsingSuccess<'v>
    | Failure of ParsingFailure

let parsePartial parser str =
    let c = InstantCombinators.ParserContext.create(str)
    match memoParse parser c with
    | InstantMatcher.Success s -> Success { consumed = s.next; value = s.value }
    | InstantMatcher.Failure f -> 
        let memo = (c :> InstantMatcher.IParseContext).memo
        Failure { index = memo.lastErrorPos; expectations = memo.expectationsFor memo.lastErrorPos }

let parse parser str = 
    let r = parsePartial parser str
    match r with
    | Success s when s.consumed <> str.Length -> Failure { index = s.consumed; expectations = ["end of input"]} 
    | _ -> r

let parser = ParseSequenceBuilder()

let production name = 
    let failingResolver = fun () -> fun c -> InstantMatcher.Failure { index = 0 }
    Parser<'a>(name, failingResolver)

type Parser<'a> = InstantCombinators.Parser<'a>
