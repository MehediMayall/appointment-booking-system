redis-cli --cluster create \
127.0.0.1:6379 \
127.0.0.1:6379 \
127.0.0.1:6379 \
127.0.0.1:6379 \
127.0.0.1:6379 \
127.0.0.1:6379 \
--cluster-replicas 1

redis-cli -p 6379 cluster nodes