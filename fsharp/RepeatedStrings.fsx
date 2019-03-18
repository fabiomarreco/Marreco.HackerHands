open FsCheck.Arb
#r "paket: nuget FsCheck //"
#load  ".fake/RepeatedStrings.fsx/intellisense.fsx"

open System

let konst a b = a

let bruteForce (s: string) letter (n:int) = 
    Seq.initInfinite (konst s) |> Seq.collect id |> Seq.take (int n)  |> Seq.toList
    |> Seq.filter (fun x-> x = letter) |> Seq.length 

//"uhabm" 'a' 9
//let s = "abct"
//let n = 10

let s, n = "uhabm", 9
let letter = 'a'
bruteForce s letter n

let repeatedStrings (s:string) letter (n:int) = 
    let countUnique = s |> Seq.filter(fun x-> x = letter) |> Seq.length |> float
    let l = s.Length |> float
    let maxCount = countUnique * (float n) / l |> truncate
    maxCount |> int


open FsCheck

let testGen = gen {
    let! n = Gen.choose (1,10)
    let! strSize = Gen.choose (1, 5)
    let! chars = Gen.listOfLength strSize (Gen.elements ['a'..'z'])
    let str = new System.String (List.toArray chars)
    return (str, n)
}

let isCorrect letter (str, n) = 
    let expected = bruteForce str letter n
    let actual = repeatedStrings str letter n 
    printfn "str: %A n: %A, expected: %A, actual: %A" str n expected actual
    expected = actual


Check.Quick (Prop.forAll (Arb.fromGen testGen) (isCorrect 'a'))

bruteForce "uhabm" 'a' 9
