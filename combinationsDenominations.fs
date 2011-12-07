// Combinations Denominations
// In how many ways can you make a given input with a set of denominations

let in = try
        float(System.Console.ReadLine())
    with
        | :? System.FormatException -> printfn "Looking for a floating point number"

let combinations input head head2 = List.sum [for z in 1.0 .. floor(input/head) -> floor(( input - (z * head)) / head2)];;
let rec cd i d = 
    match (List.sort d) with
    [] -> 0.0 // Cant make combinations out of nothing
    | [_] as g -> 1.0 // Only one way to do the last one
    | h::h2::t -> (combinations i h h2) + (cd i t);;
// Toyed with this concept below so we wernt bounded by the iterative loop if computation on j got expensive and we needed to parallelize..still think there is a better way than how i'm doing it, but, doesn't crash on large inputs
//    | h::h2::t -> (List.sumBy (combinations i h h2) [ for j in 0.0 .. floor(i/h) -> j ]) + (cd i t);;

// The tail recursive optimizations blow my mind -- I wish I would have found functional programming sooner. 
// Seems very natural to port this stuff onto GPUs as a function with many indepandant tail recursive calls could be mapped to a kernel easily (in theory)
// and update the global address space asyncronously?!?!
// I still need to explore this because there are still some details that are unclear, totally jazzed about this though.

cd 23.4 [1.0; 0.5; 0.1; 0.01];;