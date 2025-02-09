import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5182';

// Add a response interceptor
axios.interceptors.response.use(
  response => {
    // Return the response data if successful
    return response;
  },
  error => {
    // Log the error to the console
    console.error('API Error:', error.response ? error.response.data : error.message);
    
    // Optionally, you can also throw the error to allow further handling
    return Promise.reject(error);
  }
);



export default {
  getTasks: async () => {
    const result = await axios.get(`${axios.defaults.baseURL}/todolist`);
    return result.data;
  },

  addTask: async (name) => {
    
    const result = await axios.post(`${axios.defaults.baseURL}/todo`, { Name:name,IsComplete:false });
    console.log('addTask', name);
    return result;
  },

  setCompleted: async (id, isComplete) => {
    const result = await axios.put(`${axios.defaults.baseURL}/complete/${id}`, {IsComplete: isComplete});
    console.log(`complete/${id}`, isComplete);
    return result;
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`${axios.defaults.baseURL}/todolist/${id}`);
    console.log('deleteTask');
    return result;
  }
};