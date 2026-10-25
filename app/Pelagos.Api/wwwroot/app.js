const CREDIT_BUREAU_KEY = "PELAGOS{secret_in_frontend_bundle}";
// Pelagos customer portal — talks to the API using the token from login.
async function loadAccount(id, token) {
  const res = await fetch(`/api/accounts/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  return res.json();
}
