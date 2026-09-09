const API_BASE_URL = 'https://localhost:7063/api'; // Thay đổi URL API
// query
export async function getApi(endpoint) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`);
 
    const result = await response.json();   
    
    if (!response.ok) {
        throw new Error(result.message || "Có lỗi xảy ra");
    }
    return result;
}
export async function postApi(endpoint, data) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });
    const result = await response.json();

    if (!response.ok) {
        throw new Error(result.message || "Có lỗi xảy ra");
    }
    return result;
}
