# stringSorting.py -- sort an input string based on an ordering string.
# Programmer: Nick Kolegraff

# This was some very ugly code I found in my Repos...looks like a C programmer trying to write python
def csort(input_string, order_string):
   # one greater than the largest possible rank an element can have.
   magic = max(len(input_string), len(order_string)) + 1

   # dictionary scores - to keep a ranking
   dict_score = range(1,len(order_string)+1)
   order_dict = dict(zip(order_string, dict_score))

   # initialize a buffer to insert values where score=index
   output_buf = [0]*magic

   # score the input based on dictionary ranking -- insert into buf using score=insert index
   input_dict = dict(zip(input_string, [magic]*len(input_string)))
   for item in input_string:
       temp_score = order_dict.get(item, magic)
       if(temp_score < input_dict[item]):
           input_dict[item] = temp_score
           output_buf[temp_score-1] = item
       else:
           output_buf.append(item)

   return ''.join([item for item in output_buf if item != 0])


# Rethinking with an new understanding of beautiful code..
def csort_concise(input_string, order_string):
    max_order_key = max(len(input_string), len(order_string))
    order_dict = dict(zip(order_string, range(0,len(order_string))))
    return ''.join(sorted(list(input_string), key=lambda x: order_dict.get(x, max_order_key)))
