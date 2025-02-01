import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Custom metrics
const successRate = new Rate('success_rate');
const requestDuration = new Trend('request_duration');

// Configuration options
export const options = {
    // Run once with 10 VUs for 30 seconds
    vus: 10,
    duration: '30s',
    thresholds: {
        http_req_failed: ['rate<0.01'],  // Allow <1% failure rate
        http_req_duration: ['p(95)<500'], // 95% of requests should be < 500ms
        'success_rate': ['rate>0.95'],    // 95% success rate
    }
};

// Main test function
export default function () {
    const params = {
        headers: {
            'Content-Type': 'application/json'
        },
        timeout: '10s'
    };

    // Add sleep between requests to respect rate limit
    sleep(0.6);

    const response = http.get('http://backend:80/api/v1/Product', params);

    // Record metrics
    successRate.add(response.status === 200);
    requestDuration.add(response.timings.duration);

    // Check responses
    check(response, {
        'status is 200 or acceptable': r => [200, 404, 429].includes(r.status),
    });

    // Log appropriately
    if (response.status === 429) {
        console.warn(`Rate Limit Reached: ${response.status} - ${response.body}`);
    } else if (response.status === 404) {
        console.info(`No products found: ${response.status}`);
    } else if (response.status === 200) {
        console.info(`Request Success: ${response.status}`);
    } else {
        console.error(`Unexpected status: ${response.status} - ${response.body}`);
    }
}