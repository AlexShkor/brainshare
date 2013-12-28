namespace BrainShare.Infrastructure.Mongo
{
    public class PagingInfo
    {
        private int _itemsPerPage = 10;
        private int _skip = 0;
        private int _currentPage = 1;

        public int Skip
        {
            get
            {
                if (_skip == 0)
                    _skip = (CurrentPage - 1) * _itemsPerPage;
                return _skip;
            }
            set
            {
                _skip = value;
            }
        }

        public int Take
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
            }
        }

        /// <summary>
        /// Total count of items
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Actual loaded items per page 
        /// </summary>
        public int ActualLoadedItemCount { get; set; }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
            }
        }

        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return (int)(TotalCount / ItemsPerPage + ((TotalCount % ItemsPerPage > 0) ? 1 : 0));
            }
        }

        public int IndexOfFirstItem
        {
            get
            {
                return (CurrentPage - 1) * ItemsPerPage + 1;
            }
        }

        public int IndexOfLastItem
        {
            get
            {
                return (CurrentPage - 1) * ItemsPerPage + ActualLoadedItemCount;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPagesCount);
            }
        }
    }
}