from flask import Flask, request, send_file
import io
from PIL import Image
import numpy as np

app = Flask(__name__)

@app.route('/process', methods=['POST'])
def process_image():
    file = request.files['image']
    img = Image.open(file.stream)

    # 在这里进行图像处理
    processed_img = img.convert("L")  # 示例：转换为灰度图像

    img_io = io.BytesIO()
    processed_img.save(img_io, 'PNG')
    img_io.seek(0)

    return send_file(img_io, mimetype='image/png')

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)