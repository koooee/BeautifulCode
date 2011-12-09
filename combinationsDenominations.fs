// Combinations Denominations
// In how many ways can you make a given input with a set of denominations
// From PowerShell
// > & 'C:\Program Files (x86)\FSharp-2.0.0.0\bin\fsi.exe' --use:combinationsDenominations.fs

open System;;
exception DenomError;;
exception NumberTooBig;;
let combinations input head head2 (index: float) = floor ((input - (index * head)) / head2);;
let denoms_ok (input: float) (min_denom: float) = (input / min_denom) - floor (input / min_denom);;

// Calculate the Total Combinations
let rec cd i d =
    if i <= 0.0 then [0.0] else
    match List.rev (List.sort d) with
    [] -> [0.0] // Cant make combinations out of nothing
    | [h] -> [1.0] // Only one way to do one element
    | h::h2::[] -> 1.0::[]
    // I'd Imagine there is a much better way around this than how i'm doing it, list compression wont scale well
    // but wanted it to parallelize easily if need be
    | h::h2::t -> (List.sumBy (combinations i h h2) [ for j in 0.0 .. floor (i/h) -> j ])::(cd i (h2::t));;

let get_input d = 
     try
        printfn "Enter a Floating Point Number: ";
        let temp = Console.ReadLine() in
        let min = (List.min d) in
        let diff = (denoms_ok (float(temp)) min) in
        if( diff < 0.9999 && diff > 0.0001) then // Not quite sure how else to do this
           raise (DenomError)
         elif float(temp) > (2.0**20.0) then
           raise (NumberTooBig)
         else
           float(temp)
      with
        | NumberTooBig -> printfn "Number is too big too compute"; 0.0 // It should exit here but was having some trouble, still learning F# type system
        | DenomError -> printfn "Denominations need to be smaller in order to recreate input"; 0.0
        | :? System.FormatException -> printfn "Looking for a floating point number"; 0.0;;


// Now lets use our functions
// I'd imagine there is a much more elegant f# way of doing this below
let denoms = [100.0; 50.0; 20.0; 10.0; 5.0; 1.0; 0.25; 0.1; 0.05; 0.01];;
cd 23.4 denoms;;
cd 0.34 denoms;;
cd 1456.18 denoms;;
cd 1.23 denoms;;

let input = get_input denoms;;
printfn "Total Combinations: %f" (List.sum (cd input denoms));;
System.Environment.Exit(1);;

// The tail recursive optimizations blow my mind -- I wish I would have found functional programming sooner. 
// Seems very natural to port this stuff onto GPUs as a function with many indepandant tail recursive calls could be mapped to a kernel easily (in theory)
// I need to explore this more because there are still some details that are unclear, totally jazzed about this though.  I'm sold.

//Possible Issue: I'd suspect summing on floats that have been divided with give some sort of precision error
//                the exact nature of how it effects the code is unclear to me, values with be slightly off
//                it is also subject to have some inputs slip though with bad denoms since I am doing boolean
//                comparison on floats, I have yet to find such a case.

//TODO: prompt the user for denom input
//TODO: check for large numbers and throw an exception if they don't have the resources to compute it
//TODO:
// I'd imagine there is a way to tweak this so it can scale to large data AND numbers...can't put my finger on it at the moment
//     | h::h2::t -> (List.sumBy (combinations i h h2) [ for j in 0.0 .. floor(i/h) -> j ]) + (cd i t)
//TODO: the user input prompt is a bit sketchy and awkward, but wanted to see if I could get that going
//TODO: test cases arent really doing any assertations, they should, also could use a powershell script to
//      test user inputs, but this is all new to me so I am still learning how to do some of it
//TODO: need to come up with a good way to test assertations on large values of combinations