import { FiChevronLeft, FiChevronRight, FiChevronsLeft, FiChevronsRight } from 'react-icons/fi';

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    hasNextPage?: boolean;
    hasPreviousPage?: boolean;
    onPageChange: (page: number) => void;
    totalItems: number;
    pageSize: number;
}

const Pagination = ({
    currentPage,
    totalPages,
    hasNextPage = currentPage < totalPages,
    hasPreviousPage = currentPage > 1,
    onPageChange,
    totalItems,
    pageSize,
}: PaginationProps) => {
    const handlePrev = () => {
        if (hasPreviousPage) {
            onPageChange(currentPage - 1);
        }
    };

    const handleNext = () => {
        if (hasNextPage) {
            onPageChange(currentPage + 1);
        }
    };

    const getPageNumbers = () => {
        const pageNumbers = [];
        const maxPagesToShow = 5;
        const halfMaxPages = Math.floor(maxPagesToShow / 2);
        let startPage = Math.max(1, currentPage - halfMaxPages);
        let endPage = Math.min(totalPages, currentPage + halfMaxPages);

        // Ajustar cuando estamos cerca del inicio
        if (currentPage - halfMaxPages < 1) {
            endPage = Math.min(totalPages, maxPagesToShow);
        }

        // Ajustar cuando estamos cerca del final
        if (currentPage + halfMaxPages > totalPages) {
            startPage = Math.max(1, totalPages - maxPagesToShow + 1);
        }

        // Mostrar puntos suspensivos al inicio si es necesario
        if (startPage > 1) {
            pageNumbers.push('...');
        }

        for (let i = startPage; i <= endPage; i++) {
            pageNumbers.push(i);
        }

        // Mostrar puntos suspensivos al final si es necesario
        if (endPage < totalPages) {
            pageNumbers.push('...');
        }

        return pageNumbers;
    };

    const startCount = ((currentPage - 1) * pageSize) + 1;
    const endCount = Math.min(currentPage * pageSize, totalItems);

    return (
        <div className="flex flex-col sm:flex-row items-center justify-between gap-4 px-4 py-3 bg-gray-900 rounded-lg shadow-sm border border-gray-800">
            {/* Contador de items */}
             <div className="text-sm text-gray-300">
                Showing{' '}
                {startCount === endCount ? (
                    <span className="font-medium text-white">{startCount.toLocaleString()}</span>
                ) : (
                    <>
                        <span className="font-medium text-white">{startCount.toLocaleString()}</span> to{' '}
                        <span className="font-medium text-white">{endCount.toLocaleString()}</span>
                    </>
                )}
                {' '}of <span className="font-medium text-white">{totalItems.toLocaleString()}</span> results
            </div>

            {/* Controles de paginación */}
            <div className="flex items-center gap-1">
                {/* Primer página */}
                <button
                    onClick={() => onPageChange(1)}
                    disabled={!hasPreviousPage}
                    className={`p-2 rounded-md ${!hasPreviousPage ? 'text-gray-500 cursor-not-allowed' : 'text-gray-300 hover:bg-gray-800 hover:text-white'}`}
                    aria-label="First page"
                >
                    <FiChevronsLeft className="w-5 h-5" />
                </button>

                {/* Página anterior */}
                <button
                    onClick={handlePrev}
                    disabled={!hasPreviousPage}
                    className={`p-2 rounded-md ${!hasPreviousPage ? 'text-gray-500 cursor-not-allowed' : 'text-gray-300 hover:bg-gray-800 hover:text-white'}`}
                    aria-label="Previous page"
                >
                    <FiChevronLeft className="w-5 h-5" />
                </button>

                {/* Números de página */}
                <div className="flex items-center gap-1 mx-2">
                    {getPageNumbers().map((pageNumber, index) => (
                        pageNumber === '...' ? (
                            <span key={`ellipsis-${index}`} className="px-3 py-1 text-gray-400">...</span>
                        ) : (
                            <button
                                key={pageNumber}
                                onClick={() => onPageChange(pageNumber as number)}
                                className={`w-10 h-10 rounded-md flex items-center justify-center text-sm font-medium ${pageNumber === currentPage
                                    ? "bg-blue-600 text-white shadow-md"
                                    : "text-gray-300 hover:bg-gray-800 hover:text-white"
                                    }`}
                                aria-current={pageNumber === currentPage ? "page" : undefined}
                            >
                                {pageNumber}
                            </button>
                        )
                    ))}
                </div>

                {/* Página siguiente */}
                <button
                    onClick={handleNext}
                    disabled={!hasNextPage}
                    className={`p-2 rounded-md ${!hasNextPage ? 'text-gray-500 cursor-not-allowed' : 'text-gray-300 hover:bg-gray-800 hover:text-white'}`}
                    aria-label="Next page"
                >
                    <FiChevronRight className="w-5 h-5" />
                </button>

                {/* Última página */}
                <button
                    onClick={() => onPageChange(totalPages)}
                    disabled={!hasNextPage}
                    className={`p-2 rounded-md ${!hasNextPage ? 'text-gray-500 cursor-not-allowed' : 'text-gray-300 hover:bg-gray-800 hover:text-white'}`}
                    aria-label="Last page"
                >
                    <FiChevronsRight className="w-5 h-5" />
                </button>
            </div>
        </div>
    );
};

export default Pagination;