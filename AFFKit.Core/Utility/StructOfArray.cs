namespace AFFKit.Core.Utility
{
    /// <summary>
    /// <para>该数据结构约束如下：</para>
    /// <para>0. 所有数组的长度都是固定的</para>
    /// <para>1. 一共有number_of_array个数组</para>
    /// <para>2. 所有数组都是T类型</para>
    /// <para>3. 所有数组memory_capacity=size_of_array</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct StructOfArray<T>
    {
        private readonly int[,] sparse_set;
        private readonly T[,] matrix;
        public readonly int number_of_array;
        public readonly int size_of_array;
        public int Count { get; private set; }
        public StructOfArray(int number_of_array, int size_of_array)
        {
            this.Count = 0;
            this.number_of_array = number_of_array;
            this.size_of_array = size_of_array;
            this.sparse_set = new int[2, size_of_array];
            this.matrix = new T[number_of_array, size_of_array];

            for (int i = 0; i < this.size_of_array; i++)
            {
                this.sparse_set[0, i] = -1;
                this.sparse_set[1, i] = -1;
            }
        }
        public override string ToString() 
        {
            string s = "";
            s += "---StructOfArray.ToString()---\nsparse_set[0, ...](id2index): ";
            for (int i = 0; i < size_of_array; i++) 
            {
                s += sparse_set[0, i];
                s += ", ";
            }
            s += "\nsparse_set[1, ...](index2id): ";
            for (int i = 0; i < size_of_array; i++)
            {
                s += sparse_set[1, i];
                s += ", ";
            }
            s += "\nmatrix layout:\n";
            for (int i = 0; i < number_of_array; i++)
            {
                for (int j = 0; j < size_of_array; j++)
                {
                    s += matrix[i, j];
                    s += ", ";
                }
                s += "\n";
            }
            s += "---StructOfArray.ToString()---\n";
            return s;
        }

        /// <summary>
        /// <para>Return first available id in sparse set.</para>
        /// <para>if no available exist, return -1</para>
        /// </summary>
        /// <returns></returns>
        public int get_available_id()
        {
            for (int i=0; i<this.number_of_array; i++)
            {
                if (this.sparse_set[0, i] == -1)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// <para>API: add</para>
        /// <para>if success: return id of new_value</para>
        /// <para>if failed: return -1</para>
        /// <para>if id is not valid, this.get_available_id() is automatically called</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">Expect: new_value.Length == this.number_of_array</param>
        /// <returns></returns>
        public int add(int id, T[] value)
        {
            if (validate_id(id))
            {
                id = get_available_id();
            }
            if (id == -1)
            {
                return -1;
            }
            for (int i=0; i<this.number_of_array; i++)
            {
                this.matrix[i, this.Count] = value[i];
            }
            sparse_set[0, id] = this.Count;
            sparse_set[1, this.Count] = id;
            this.Count += 1;
            return id;
        }
        /// <summary>
        /// <para>API: remove</para>
        /// <para>if return 0: success</para>
        /// <para>if return 1: out of bound</para>
        /// </summary>
        /// <param name="id"></param>
        public int remove(int id)
        {
            if (!validate_id(id))
                return 1;
            return remove_by_index(this.get_index(id));
        }
        public int remove_by_index(int index)
        {
            if (!validate_index(index))
                return 1;
            // These codes are order-sensitive.
            // Be aware of modification.
            #region remove logic
            int last_index = this.Count - 1;
            int remove_id = get_id(index);
            int last_id = get_id(last_index);
            for (int i = 0; i < this.number_of_array; i++)
            {
                matrix[i, index] = matrix[i, last_index];
            }
            this.sparse_set[0, remove_id] = -1;
            this.sparse_set[1, index] = last_id;
            this.sparse_set[0, last_id] = index;
            // this.sparse_set[1, last_index] = -1; // Optional: the value is out of bound.
            this.Count -= 1;
            #endregion
            return 0;
        }
        /// <summary>
        /// <para>API: modify</para>
        /// <para>if return 0: success</para>
        /// <para>if return 1: out of bound</para>
        /// <para>if update_value.Length is greater than this.number_of_array:</para>
        /// <para>then overflow will be write into default values</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int modify(int id, T[] value)
        {
            if (!this.validate_id(id))
                return 1;
            return this.modify_by_index(this.get_index(id), value);
        }
        public int modify_by_index(int index, T[] value)
        {
            if (!this.validate_index(index))
                return 1;
            for (int i=0; i<this.number_of_array; i++)
            {
                this.matrix[i, index] = (i >= value.Length ? default(T) : value[i])!;
            }
            return 0;
        }
        /// <summary>
        /// <para>Return an array that represent a row of matrix</para>
        /// <para>row of matrix = ith element of struct array</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T[] query(int id)
        {
            return this.query_by_index(get_index(id));
        }
        public T[] query_by_index(int index)
        {
            if (!validate_index(index))
            {
                throw new ArgumentException("index is out of range");
            }
            T[] result = new T[this.number_of_array];
            for (int i=0; i<this.number_of_array; i++)
            {
                result[i] = this.matrix[i, index];
            }
            return result;
        }
        public int get_index(int id)
        {
            return sparse_set[0, id];
        }
        public int get_id(int index)
        {
            if (this.validate_index(index))
            {
                return sparse_set[1, index];
            }
            else
            {
                return -1;
            }
        }
        private bool validate_index(int index)
        {
            return (0 <= index && index <= this.Count - 1);
        }
        /// <summary>
        /// <para>Check if id is used</para>
        /// <para>return true if id2index[id] is not -1</para>
        /// <para>return false if id2index[id] is -1</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool validate_id(int id)
        {
            if (0 <= id && id <= this.size_of_array - 1)
            {
                return (this.sparse_set[0, id] != -1);
            }
            else
            {
                return false;
            }
        }
    }
}
