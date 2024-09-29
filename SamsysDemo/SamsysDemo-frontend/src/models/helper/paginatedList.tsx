interface PaginatedResponse<T> {   
    totalRecords:number;
    count:number,
    items: T[];
    pageSize:number;
    totalPages:number;
    currentPage:number;
    hasNextPage:boolean;
    hasPreviousPage:boolean;

  }

  function createEmptyPaginatedResponse<T>(): PaginatedResponse<T> {
    return {
      totalRecords: 0,
      count: 0,
      items: [],
      pageSize: 0,
      totalPages: 0,
      currentPage: 0,
      hasNextPage: false,
      hasPreviousPage: false
    };
  }